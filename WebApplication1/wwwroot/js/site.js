let butonDurum = {
    mw5: {},  // MW5 butonlarının durumu
    mw6: {}   // MW6 butonlarının durumu
};

function mw5Komut(bitIndex) {
    let durum = butonDurum.mw5[bitIndex] || false;
    let komut = durum ? 'kapat' : 'ac';
    butonDurum.mw5[bitIndex] = !durum;

    fetch(`/PLC/MW5BitAyarla?bitIndex=${bitIndex}&komut=${komut}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateStatusText(`MW5 Bit ${bitIndex} ${komut.toUpperCase()} oldu!`);
                updateWord();  // Word dosyasını güncelle
            } else {
                updateStatusText(`Hata: ${data.message}`);
            }
        })
        .catch(error => {
            console.error('Hata:', error);
            updateStatusText("Bağlantı hatası!");
        });
}

function mw6Komut(bitIndex) {
    let durum = butonDurum.mw6[bitIndex] || false;
    let komut = durum ? 'kapat' : 'ac';
    butonDurum.mw6[bitIndex] = !durum;

    fetch(`/PLC/MW6BitAyarla?bitIndex=${bitIndex}&komut=${komut}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateStatusText(`MW6 Bit ${bitIndex} ${komut.toUpperCase()} oldu!`);
                updateWord();  // Word dosyasını güncelle
            } else {
                updateStatusText(`Hata: ${data.message}`);
            }
        })
        .catch(error => {
            console.error('Hata:', error);
            updateStatusText("Bağlantı hatası!");
        });
}

function updateWord() {
    fetch('/PLC/WordKaydetVeGonder')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log("Word dosyası güncellendi ve PLC'ye gönderildi.");
            } else {
                console.log("Hata: " + data.message);
            }
        })
        .catch(error => {
            console.error("Bir hata oluştu.", error);
        });
}

document.addEventListener("contextmenu", (e) => e.preventDefault());
document.addEventListener("keydown", (e) => {
    if (["F12", "U"].includes(e.key) && e.ctrlKey) e.preventDefault();
    if (e.ctrlKey && e.shiftKey && e.key === "I") e.preventDefault();
});
