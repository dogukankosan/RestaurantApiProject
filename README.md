# 🍽️ RestaurantApiProject

![License](https://img.shields.io/github/license/dogukankosan/RestaurantApiProject)
![Stars](https://img.shields.io/github/stars/dogukankosan/RestaurantApiProject)
![Issues](https://img.shields.io/github/issues/dogukankosan/RestaurantApiProject)
![Last Commit](https://img.shields.io/github/last-commit/dogukankosan/RestaurantApiProject)

> **RestaurantApiProject**, restoran yönetimi için hazırlanmış modern bir **ASP.NET Core Web API** projesidir. Admin paneli, dinamik içerikler, rezervasyonlar, menü yönetimi ve daha fazlası için güçlü bir altyapı sunar.

---

## 🔗 Canlı Linkler

- 🎯 **Admin Panel UI**: [restoran.runasp.net](http://restoran.runasp.net)
- ⚙️ **API Dokümantasyon (Swagger)**: [restoranapi.runasp.net/swagger/index.html](http://restoranapi.runasp.net/swagger/index.html)
- 📦 **Proje Repo**: [github.com/dogukankosan/RestaurantApiProject](https://github.com/dogukankosan/RestaurantApiProject)

---

## 🚀 Temel Özellikler

- 📦 **Modüler yapı**: Hakkımızda, Şefler, Ürünler, Kategoriler, Mesajlar, Rezervasyonlar vb.
- 🖼️ **Base64 image upload** desteği
- 🧑‍🍳 **Admin Panel** ile tam kontrol: ürün ekleme, güncelleme, silme
- 📅 **Rezervasyon yönetimi** ve durum takibi
- 🌍 **API First** yaklaşımıyla yapılandırılmış backend

---

## 🗂️ Proje Yapısı

```bash
RestaurantApiProject/
├── Controllers/          # API controller sınıfları (örneğin: AdminChefController, AdminProductController)
├── Dtos/                 # Veri transfer nesneleri (DTOs)
├── Models/               # Entity sınıfları
├── Mappings/             # AutoMapper profilleri
├── FluentValidation/     # Tüm model doğrulama kuralları
├── wwwroot/              # Statik dosyalar (görseller)
├── Program.cs / Startup.cs  # Servis konfigürasyonu ve middleware ayarları
```

---

## 🧪 Denemek için

### 1. Projeyi Klonla

```bash
git clone https://github.com/dogukankosan/RestaurantApiProject.git
cd RestaurantApiProject
```

### 2. Veritabanı Ayarı
- **connection string**: `appsettings.json` içinde ayarlı.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=RestaurantDB.mssql.somee.com;Database=RestaurantDB;user id=restoranapi_SQLLogin_1;pwd=***;TrustServerCertificate=True;"
}
```

> İsteğe bağlı olarak MSSQL Express ya da Azure üzerinde kendi veritabanını kurabilirsin.

### 3. Projeyi Çalıştır (Visual Studio ile)

- `F5` ile başlat.
- Swagger otomatik olarak açılır: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

---

## 📸 Ekran Görüntüleri
<img width="1919" height="1039" alt="image" src="https://github.com/user-attachments/assets/16a52f20-2995-40af-9fe8-640a4b4dc33a" />
<img width="1901" height="964" alt="image" src="https://github.com/user-attachments/assets/c587d161-cade-4d50-b07f-fccee5b341c4" />

---

## ✅ Geliştirici Notları

- Kullanılan Teknolojiler:
  - ASP.NET Core 7+
  - Entity Framework Core
  - AutoMapper
  - FluentValidation
  - Swagger
  - SweetAlert2 (UI tarafında)

---

## 🤝 Katkı

Bu projeye katkıda bulunmak istersen:

1. Fork'la 🍴
2. Yeni bir dal oluştur (`git checkout -b feature/özellik`)
3. Değişiklik yap ve commit'le (`git commit -m 'özellik eklendi'`)
4. Push et (`git push origin feature/özellik`)
5. Pull Request gönder

---

## 📄 Lisans

MIT Lisansı – detaylar için `LICENSE` dosyasına göz atabilirsin.

---

## 📬 İletişim

- 👨‍💻 Geliştirici: [@dogukankosan](https://github.com/dogukankosan)
- 🐛 Hatalar veya öneriler için: [Issues sekmesi](https://github.com/dogukankosan/RestaurantApiProject/issues)

---

<p align="center">
  <img src="https://img.shields.io/badge/ASP.NET%20Core-API-blue?logo=dotnet" />
  <img src="https://img.shields.io/badge/SQL%20Server-Database-red?logo=microsoftsqlserver" />
  <img src="https://img.shields.io/badge/Somee.com-Hosting-green" />
</p>
