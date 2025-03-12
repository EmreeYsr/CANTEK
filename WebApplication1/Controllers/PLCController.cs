using EasyModbus;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication1.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;

namespace WebApplication1.Controllers
{
    public class PLCController : Controller
    {
        private readonly ModbusClient PLC;

        public PLCController()
        {
            PLC = new ModbusClient("192.168.0.10", 502);
        }

        [HttpGet("PLC/BaglantiAc")]
        public IActionResult BaglantiAc()
        {
            try
            {
                if (!PLC.Connected)
                {
                    PLC.Connect();
                }
                return Json(new { success = true, message = "Bağlantı sağlandı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }

        [HttpGet("PLC/BaglantiKapat")]
        public IActionResult BaglantiKapat()
        {
            try
            {
                if (PLC.Connected)
                {
                    PLC.Disconnect();
                }
                return Json(new { success = true, message = "Bağlantı kesildi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bağlantı kesme hatası: {ex.Message}" });
            }
        }

        [HttpGet("PLC/MW5BitAyarla")]
        public IActionResult MW5BitAyarla(int bitIndex, string komut)
        {
            try
            {
                if (!PLC.Connected)
                {
                    PLC.Connect();
                }

                if (bitIndex < 0 || bitIndex > 15)
                {
                    return Json(new { success = false, message = "Geçersiz bit indeksi!" });
                }

                // Mevcut MW5 değerini oku
                int mwDeger = PLC.ReadHoldingRegisters(5, 1)[0];
                int yeniDeger = mwDeger;

                // Komut'a göre bit açma veya kapama işlemi
                if (komut == "ac")
                {
                    yeniDeger |= (1 << bitIndex);  // Bit aç
                }
                else if (komut == "kapat")
                {
                    yeniDeger &= ~(1 << bitIndex);  // Bit kapat
                }

                // Yeni değeri PLC'ye yaz
                PLC.WriteSingleRegister(5, yeniDeger);

                // Word dosyasını güncelle
                WordKaydetVeGonder();

                return Json(new { success = true, MWDegeri = yeniDeger, BinaryMW = Convert.ToString(yeniDeger, 2).PadLeft(16, '0') });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }

        [HttpGet("PLC/MW6BitAyarla")]
        public IActionResult MW6BitAyarla(int bitIndex, string komut)
        {
            try
            {
                if (!PLC.Connected)
                {
                    PLC.Connect();
                }

                if (bitIndex < 0 || bitIndex > 15)
                {
                    return Json(new { success = false, message = "Geçersiz bit indeksi!" });
                }

                // Mevcut MW6 değerini oku
                int mwDeger = PLC.ReadHoldingRegisters(6, 1)[0];
                int yeniDeger = mwDeger;

                // Komut'a göre bit açma veya kapama işlemi
                if (komut == "ac")
                {
                    yeniDeger |= (1 << bitIndex);  // Bit aç
                }
                else if (komut == "kapat")
                {
                    yeniDeger &= ~(1 << bitIndex);  // Bit kapat
                }

                // Yeni değeri PLC'ye yaz
                PLC.WriteSingleRegister(6, yeniDeger);

                // Word dosyasını güncelle
                WordKaydetVeGonder();

                return Json(new { success = true, MWDegeri = yeniDeger, BinaryMW = Convert.ToString(yeniDeger, 2).PadLeft(16, '0') });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }

        public IActionResult Okuma()
        {
            try
            {
                if (!PLC.Connected)
                {
                    PLC.Connect();
                }

                if (PLC.Connected)
                {
                    int[] read = PLC.ReadHoldingRegisters(0, 10);

                    int mw5Deger = read[5];
                    int mw6Deger = read[6];

                    return Json(new { success = true, mw5 = mw5Deger, mw6 = mw6Deger });
                }
                else
                {
                    return Json(new { success = false, message = "Bağlantı yok" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult WordKaydetVeGonder()
        {
            try
            {
                ModbusClient modbus = new ModbusClient("192.168.0.10", 502);
                modbus.Connect();

                int[] mw5Degeri = modbus.ReadHoldingRegisters(5, 1);
                int[] mw6Degeri = modbus.ReadHoldingRegisters(6, 1);

                modbus.Disconnect();

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MWVerileri.docx");

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();

                    Paragraph para1 = new Paragraph(new Run(new Text($"MW5 Değeri: {mw5Degeri[0]}")));
                    Paragraph para2 = new Paragraph(new Run(new Text($"MW6 Değeri: {mw6Degeri[0]}")));

                    body.Append(para1);
                    body.Append(para2);

                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }

                WordDosyasiniPLCGonder(filePath);

                return Json(new { success = true, message = "Word dosyası kaydedildi ve PLC'ye gönderildi.", filePath = "/MWVerileri.docx" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public void WordDosyasiniPLCGonder(string localFilePath)
        {
            string plcIp = "192.168.0.10";  // PLC'nin IP adresi
            string remotePath = "/plc_storage/MWVerileri.docx"; // PLC'deki hedef dizin

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("kullanici", "sifre"); // PLC'nin FTP bilgileri
                client.UploadFile($"ftp://{plcIp}{remotePath}", WebRequestMethods.Ftp.UploadFile, localFilePath);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        // İngilizce sayfa
        public IActionResult IndexEn()
        {
            return View();
        }
    }
}
