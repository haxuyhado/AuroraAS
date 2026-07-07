# 🏥 AuroraAS

### Автоматизированная система учёта заказов медицинского оборудования

---

## 📋 О проекте

Система для автоматизации учёта, обработки и отслеживания заказов медицинского оборудования. Предназначена для повышения эффективности работы медицинских учреждений и поставщиков.

---

## 🧩 Состав проекта

### 1. База Данных (AuroraDataBase.sql) 
- **СУБД:** MS SQL Server
- Скрипт развертывания

---

### 2. API 

#### 2.1 MedSaleAS_API 
- **Платформа:** ASP.NET Framework 4.7.2
- **Основной язык:** C#
- **Дополнительно:** HTML, CSS, JS, Entity Framework 6
> ⚠️ Переписана на .NET Core (см. п. 2.2)

#### 2.2 MedSale_API_Core
- **Платформа:** ASP.NET Core 8
- **Основной язык:** C#
- **Дополнительно:** HTML, CSS, EF Core

---

### 3. Приложение для сотрудников (MedSaleAS_Windows)
- **Платформа:** WPF Framework 4.7.2
- **Основные языки:** C#, XAML

---

## 🛠️ Технологии

- **Backend:** C#, ASP.NET Core 8, Entity Framework Core
- **Frontend:** HTML, CSS, JavaScript
- **Desktop:** C#, WPF, XAML
- **Database:** MS SQL Server
- **Version Control:** Git, GitHub