# WebApplication1 - ASP.NET MVC PLC Yönetim Sistemi

Bu proje, ASP.NET MVC mimarisi ile geliştirilmiş bir kullanıcı yönetim sistemidir. Projede kullanıcı girişi, rol tabanlı erişim kontrolü ve sayfa yönlendirme gibi temel işlevler bulunmaktadır. Veritabanı olarak SQLite kullanılmaktadır.

## Özellikler

- 🔐 **Giriş Sistemi:** Kullanıcılar kullanıcı adı, şifre veya e-posta ile giriş yapabilir.
- 👤 **Rol Tabanlı Yönlendirme:** Kullanıcının rolüne göre Admin veya PLC sayfalarına yönlendirme yapılır.
- 🛠️ **SQLite Veritabanı:** Proje, platformlar arası taşınabilirlik açısından SQLite veritabanı kullanmaktadır.
- 📋 **Kullanıcı Yönetimi:** Admin, kullanıcıları görüntüleyebilir, silebilir ve rollerini yönetebilir.

## Sayfa ve Controller Yapısı

- `LoginController.cs`: Giriş işlemleri
- `AdminController.cs`: Kullanıcı yönetimi (Admin yetkili)
- `PLCController.cs`: PLC sayfalarına yönlendirme

### Görünümler (Views)

- `Views/Admin/Index`: Kullanıcı listeleme ve yönetim ekranı
- `Views/PLC/Index`: Ana PLC sayfası
- `Views/PLC/IndexHidrolik`: PLC Hidrolik ekranı

## Kurulum

1. Bu projeyi klonlayın:

   ```bash
   git clone https://github.com/kullaniciAdi/WebApplication1.git
   ```

2. Projeyi Visual Studio ile açın.
3. `App_Data` klasörü altındaki SQLite veritabanı dosyasının bulunduğundan emin olun.
4. `web.config` içindeki bağlantı cümlesini SQLite'a göre yapılandırın.
5. Projeyi çalıştırın (`Ctrl + F5`).

## Gereksinimler

- Visual Studio 2019 veya üzeri
- .NET Framework (MVC uyumlu sürüm)
- System.Data.SQLite (NuGet paketi)

## Geliştirici

Bu proje **Emre Yaşar** tarafından geliştirilmiştir.

📧 İletişim: ysr.emre.07@gmail.com
