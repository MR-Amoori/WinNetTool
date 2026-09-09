# DNS Network Utility

> یک ابزار کاربردی، سریع و سبک برای بررسی وضعیت DNS Serverها، اندازه‌گیری زمان پاسخ Ping و مدیریت آسان تنظیمات کارت شبکه در ویندوز.

---

## 🇮🇷 فارسی

### معرفی پروژه

`DNS Network Utility` یک برنامه دسکتاپ ویندوزی است که با هدف ساده‌سازی بررسی و مقایسه DNS Serverها طراحی شده است.

این برنامه به کاربر اجازه می‌دهد DNSهای مختلف را بررسی کرده، زمان پاسخ آن‌ها را اندازه‌گیری کند و DNSهای سریع‌تر را براساس امتیاز مرتب‌سازی مشاهده نماید.

در کنار قابلیت بررسی DNS، امکان بازکردن مستقیم تنظیمات کارت شبکه ویندوز و دسترسی سریع به وب‌سایت سازنده نیز در برنامه قرار گرفته است.

---

## ✨ قابلیت‌های اصلی

- بررسی زمان پاسخ DNS Serverها با استفاده از Ping
- تست DNS Serverها به‌صورت ترتیبی و یکی‌یکی
- تعیین Timeout مستقل برای هر آدرس
- Timeout پیش‌فرض برابر با ۱۵۰۰ میلی‌ثانیه
- نمایش زمان پاسخ برحسب میلی‌ثانیه
- نمایش وضعیت DNSهایی که پاسخی دریافت نکرده‌اند
- جلوگیری از نمایش پاسخ‌های ناقص یا نادرست
- مرتب‌سازی DNSها براساس کمترین زمان پاسخ
- قرارگرفتن DNSهای بدون پاسخ در انتهای فهرست
- پشتیبانی از DNS اولیه و ثانویه
- نمایش خروجی خوانا و قابل فهم برای کاربر
- امکان بازکردن مستقیم پنجره Change adapter settings
- امکان بازکردن مستقیم وب‌سایت سازنده
- رابط کاربری ساده و مناسب استفاده روزمره
- مدیریت خطاهای مربوط به شبکه و سیستم
- عدم نیاز به نصب ابزار جانبی برای اجرای قابلیت‌های اصلی

---

## 🧪 نحوه بررسی DNSها

برنامه هر DNS Server را به‌صورت ترتیبی بررسی می‌کند.

برای هر آدرس، مراحل زیر انجام می‌شود:

1. ارسال درخواست Ping به DNS اولیه
2. انتظار حداکثر تا ۱۵۰۰ میلی‌ثانیه
3. ثبت زمان پاسخ در صورت موفقیت
4. ثبت وضعیت `بدون پاسخ` در صورت Timeout یا خطا
5. بررسی DNS ثانویه، در صورت وجود
6. رفتن به DNS بعدی
7. مرتب‌سازی نهایی براساس کمترین زمان پاسخ

این روش باعث می‌شود درخواست‌ها هم‌زمان و کنترل‌نشده ارسال نشوند و نتیجه هر DNS با دقت بیشتری ثبت شود.

---

## 📊 نمونه خروجی

```text
Google DNS | 24 8.8.8.8 | 31 8.8.4.4
Cloudflare | 18 1.1.1.1 | 22 1.0.0.1
Custom DNS | بدون پاسخ 192.0.2.1 | - -
```

عددهای نمایش‌داده‌شده برحسب میلی‌ثانیه هستند.

برای مثال:

```text
18 1.1.1.1
```

یعنی زمان پاسخ آدرس `1.1.1.1` برابر با ۱۸ میلی‌ثانیه بوده است.

---

## ⚠️ نکته مهم درباره Ping

ممکن است یک DNS Server به درخواست‌های واقعی DNS پاسخ بدهد، اما به درخواست ICMP Ping پاسخ ندهد.

در این شرایط، برنامه ممکن است آن DNS را به‌صورت `بدون پاسخ` نمایش دهد؛ در حالی که سرویس DNS آن سرور همچنان فعال باشد.

دلایل احتمالی عبارت‌اند از:

- مسدودبودن ICMP توسط سرور
- محدودیت فایروال
- محدودیت سرویس‌دهنده اینترنت
- فعال‌بودن VPN یا Proxy
- قطع یا ناپایداری شبکه
- در دسترس نبودن آدرس مقصد

بنابراین مقدار Ping بیشتر نشان‌دهنده وضعیت مسیر شبکه و سرعت پاسخ ICMP است و الزاماً عملکرد کامل DNS Query را نشان نمی‌دهد.

---

## 🖥️ گزینه Change adapter settings

با انتخاب گزینه:

```text
Change adapter settings
```

برنامه پنجره تنظیمات کارت‌های شبکه ویندوز را باز می‌کند.

این پنجره معادل اجرای دستور زیر است:

```text
ncpa.cpl
```

از این بخش می‌توان کارت شبکه‌های موجود را مشاهده و مدیریت کرد؛ برای مثال:

- فعال یا غیرفعال‌کردن Network Adapter
- ورود به تنظیمات IPv4 و IPv6
- تغییر DNS
- مشاهده وضعیت اتصال
- دسترسی به تنظیمات Ethernet و Wi-Fi

---

## 🌐 وب‌سایت سازنده

با انتخاب گزینه:

```text
برنامه‌نویس
```

وب‌سایت سازنده در مرورگر پیش‌فرض سیستم باز می‌شود:

[https://mramoori.ir](https://mramoori.ir)

---

## 🚀 نحوه استفاده

### ۱. اجرای برنامه

پس از اجرای برنامه، فهرست DNS Serverها یا تنظیمات مربوط به آن‌ها را مشاهده می‌کنید.

### ۲. شروع بررسی

روی گزینه بررسی یا Ping کلیک کنید.

برنامه DNSها را یکی‌یکی آزمایش می‌کند و زمان پاسخ هرکدام را ثبت می‌نماید.

### ۳. تحلیل نتایج

- عدد کمتر نشان‌دهنده پاسخ سریع‌تر است.
- مقدار `بدون پاسخ` یعنی در زمان تعیین‌شده پاسخی دریافت نشده است.
- DNSهای سریع‌تر در ابتدای فهرست قرار می‌گیرند.
- DNSهای بدون پاسخ در انتهای فهرست قرار می‌گیرند.

### ۴. بازکردن تنظیمات کارت شبکه

برای ورود مستقیم به تنظیمات Network Adapter، روی گزینه زیر کلیک کنید:

```text
Change adapter settings
```

### ۵. ورود به وب‌سایت سازنده

برای بازکردن سایت سازنده، روی گزینه زیر کلیک کنید:

```text
برنامه‌نویس
```

---

## 🧱 ساختار پیشنهادی پروژه

```text
DNSNetworkUtility/
│
├── DNSNetworkUtility.sln
│
├── DNSNetworkUtility/
│   │
│   ├── Program.cs
│   ├── MainForm.cs
│   ├── MainForm.Designer.cs
│   ├── MainForm.resx
│   │
│   ├── NetworkActions.cs
│   ├── Models/
│   │   ├── DnsServerInfo.cs
│   │   └── DnsPingResult.cs
│   │
│   ├── Helpers/
│   │   ├── ProcessHelper.cs
│   │   └── ValidationHelper.cs
│   │
│   ├── Resources/
│   │   └── icons/
│   │
│   └── Properties/
│       ├── Resources.resx
│       └── Settings.settings
│
├── README.md
├── LICENSE
├── .gitignore
└── screenshots/
    ├── main-window.png
    └── dns-result.png
```

> ساختار دقیق پوشه‌ها ممکن است براساس ساختار فعلی پروژه شما متفاوت باشد.

---

## 🛠️ تکنولوژی‌های استفاده‌شده

| فناوری | کاربرد |
|---|---|
| C# | زبان اصلی توسعه |
| .NET / .NET 8 | بستر اجرای برنامه |
| Windows Forms | طراحی رابط کاربری دسکتاپ |
| `System.Net.NetworkInformation` | ارسال Ping و دریافت نتیجه |
| `System.Diagnostics` | اجرای ابزارهای ویندوز و بازکردن لینک |
| LINQ | مرتب‌سازی و پردازش نتایج |
| Visual Studio | محیط توسعه |
| Git | مدیریت نسخه‌ها |

---

## 💻 زبان برنامه‌نویسی

زبان اصلی این پروژه:

```text
C#
```

این پروژه با تمرکز بر موارد زیر توسعه داده شده است:

- برنامه‌نویسی شیءگرا
- مدیریت خطا
- برنامه‌نویسی Async
- جداسازی منطق شبکه از رابط کاربری
- خوانایی و نگهداری آسان کد
- تجربه کاربری ساده و کاربردی

---

## ⚙️ پیش‌نیازهای اجرای پروژه

برای اجرای نسخه توسعه‌ای پروژه به موارد زیر نیاز دارید:

- سیستم‌عامل Windows
- Visual Studio 2022 یا نسخه جدیدتر
- .NET SDK متناسب با Target Framework پروژه
- دسترسی شبکه برای آزمایش DNSها

در صورت استفاده از `.NET 8`، پیشنهاد می‌شود Runtime زیر نصب باشد:

```text
.NET 8 Desktop Runtime
```

---

## 🔧 اجرای پروژه در محیط توسعه

ابتدا مخزن پروژه را دریافت کنید:

```bash
git clone https://github.com/USERNAME/DNSNetworkUtility.git
```

وارد پوشه پروژه شوید:

```bash
cd DNSNetworkUtility
```

سپس پروژه را restore کنید:

```bash
dotnet restore
```

برای ساخت پروژه:

```bash
dotnet build --configuration Release
```

برای اجرای پروژه:

```bash
dotnet run --project DNSNetworkUtility
```

همچنین می‌توانید فایل Solution را با Visual Studio باز کرده و پروژه را از طریق آن اجرا کنید.

---

## 📦 ساخت نسخه Release

برای ساخت خروجی معمولی:

```bash
dotnet build --configuration Release
```

خروجی معمولاً در مسیر زیر قرار می‌گیرد:

```text
bin/Release/
```

---

## 📤 انتشار نسخه مستقل برای ویندوز

برای ساخت نسخه مستقل ۶۴ بیتی ویندوز:

```bash
dotnet publish DNSNetworkUtility.csproj \
  --configuration Release \
  --runtime win-x64 \
  --self-contained true \
  --output publish/win-x64
```

در Windows Command Prompt می‌توانید دستور را بدون علامت `\` اجرا کنید:

```cmd
dotnet publish DNSNetworkUtility.csproj --configuration Release --runtime win-x64 --self-contained true --output publish\win-x64
```

در این حالت کاربر نهایی برای اجرای برنامه به نصب جداگانه .NET Runtime نیاز نخواهد داشت.

---

## 🗜️ ساخت فایل ZIP انتشار

در PowerShell:

```powershell
Compress-Archive `
  -Path .\publish\win-x64\* `
  -DestinationPath .\publish\DNSNetworkUtility-win-x64.zip `
  -Force
```

فایل نهایی برای انتشار در مسیر زیر ساخته می‌شود:

```text
publish/DNSNetworkUtility-win-x64.zip
```

---

## 🔐 مجوز پروژه

این پروژه تحت مجوزی منتشر می‌شود که در فایل `LICENSE` قرار گرفته است.

در صورتی که مجوز مشخصی انتخاب نشده باشد، پیشنهاد می‌شود قبل از عمومی‌کردن مخزن یکی از مجوزهای زیر را انتخاب کنید:

- MIT License
- Apache License 2.0
- GNU GPL v3.0

---

## 👨‍💻 درباره سازنده

این پروژه توسط **محمدرضا عموری** توسعه داده شده است.

محمدرضا عموری:

- دانشجوی مهندسی کامپیوتر
- دانشجوی مقطع کاردانی مهندسی نرم‌افزار
- فعال حوزه برنامه‌نویسی و فناوری
- علاقه‌مند به حوزه دیجیتال، تربیت و روان‌شناسی
- ورزشکار رشته والیبال
- رتبه ۶۲ کنکور سراسری ایران

وب‌سایت رسمی:

[https://mramoori.ir](https://mramoori.ir)

---

## 📌 وضعیت توسعه

این پروژه در حال توسعه است و ممکن است در نسخه‌های آینده قابلیت‌های زیر به آن اضافه شود:

- تست واقعی DNS Query در کنار ICMP Ping
- پشتیبانی از IPv6
- ذخیره نتایج آزمایش‌ها
- نمایش نمودار سرعت پاسخ DNSها
- امکان اضافه‌کردن DNS سفارشی
- خروجی گرفتن در قالب CSV یا JSON
- تشخیص خودکار بهترین DNS
- پشتیبانی از چند زبان
- پوسته تاریک
- سیستم به‌روزرسانی خودکار

---

## 🤝 مشارکت در توسعه

برای مشارکت در توسعه پروژه:

1. مخزن را Fork کنید.
2. یک Branch جدید بسازید.
3. تغییرات خود را اعمال کنید.
4. تست‌های لازم را انجام دهید.
5. Commit مناسب ایجاد کنید.
6. یک Pull Request ارسال کنید.

نمونه:

```bash
git checkout -b feature/add-dns-query-test
git add .
git commit -m "feat: add real DNS query testing"
git push origin feature/add-dns-query-test
```

---

## 🐛 گزارش خطا

در صورت مشاهده خطا، اطلاعات زیر را در Issue درج کنید:

- نسخه ویندوز
- نسخه برنامه
- نوع اتصال اینترنت
- فعال‌بودن یا نبودن VPN
- متن کامل خطا
- مراحل بازتولید خطا
- تصویر یا فایل Log در صورت وجود

---

## 📄 سلب مسئولیت

این برنامه برای بررسی و تحلیل وضعیت پاسخ DNS Serverها طراحی شده است.

نتایج Ping ممکن است تحت تأثیر شرایط شبکه، فایروال، VPN، ISP و سیاست‌های سرور مقصد قرار بگیرند.

استفاده از نتایج برنامه برای تغییر تنظیمات شبکه برعهده کاربر است.

---

# DNS Network Utility

> A lightweight and practical Windows desktop application for testing DNS servers, measuring Ping response times, and accessing network adapter settings.

---

## 🇬🇧 English

### Project Overview

`DNS Network Utility` is a Windows desktop application designed to simplify DNS server testing and comparison.

The application allows users to test different DNS servers, measure their Ping response times, and sort them based on their response performance.

It also provides quick access to the Windows Network Connections panel and the developer's official website.

---

## ✨ Main Features

- Test DNS servers using ICMP Ping
- Test DNS servers sequentially, one by one
- Configurable timeout per address
- Default timeout of 1500 milliseconds
- Display response times in milliseconds
- Display DNS servers with no response
- Prevent incomplete or invalid results
- Sort DNS servers by the lowest response time
- Move non-responsive DNS servers to the end of the list
- Support for primary and secondary DNS addresses
- Clear and readable output
- Direct access to Windows Change adapter settings
- Direct access to the developer's website
- Simple and user-friendly interface
- Network and system error handling
- No additional tools required for the core features

---

## 🧪 DNS Testing Process

The application tests DNS servers sequentially.

For each address, the following process is performed:

1. Send a Ping request to the primary DNS address.
2. Wait for a maximum of 1500 milliseconds.
3. Store the response time if the request succeeds.
4. Mark the address as `No response` if it times out or fails.
5. Test the secondary DNS address if available.
6. Continue with the next DNS server.
7. Sort all results by the lowest response time.

This approach prevents uncontrolled parallel requests and makes it easier to associate every result with the correct DNS server.

---

## 📊 Example Output

```text
Google DNS | 24 8.8.8.8 | 31 8.8.4.4
Cloudflare | 18 1.1.1.1 | 22 1.0.0.1
Custom DNS | No response 192.0.2.1 | - -
```

All response times are displayed in milliseconds.

For example:

```text
18 1.1.1.1
```

means that the response time for `1.1.1.1` was 18 milliseconds.

---

## ⚠️ Important Note About Ping

A DNS server may respond to actual DNS queries while blocking ICMP Ping requests.

In this situation, the application may display the DNS server as `No response`, even though its DNS service is working correctly.

Possible reasons include:

- ICMP is blocked by the server
- Firewall restrictions
- ISP limitations
- Active VPN or Proxy
- Network instability
- Destination address unavailable

Therefore, Ping results primarily represent the ICMP network path and response speed. They do not necessarily represent the complete performance of DNS queries.

---

## 🖥️ Change Adapter Settings

By clicking:

```text
Change adapter settings
```

the application opens the Windows Network Connections panel.

This is equivalent to running:

```text
ncpa.cpl
```

From this panel, users can:

- Enable or disable network adapters
- Configure IPv4 and IPv6
- Change DNS settings
- View connection status
- Manage Ethernet and Wi-Fi adapters

---

## 🌐 Developer Website

By clicking:

```text
Developer
```

the developer's official website opens in the system default browser:

[https://mramoori.ir](https://mramoori.ir)

---

## 🚀 How to Use

### 1. Run the application

After launching the application, the DNS server list or related configuration options will be displayed.

### 2. Start testing

Click the Ping or Test button.

The application will test DNS servers one by one and record the response time for each address.

### 3. Analyze the results

- A lower value means a faster response.
- `No response` means that no response was received within the configured timeout.
- Faster DNS servers are placed at the top of the list.
- Non-responsive DNS servers are placed at the bottom.

### 4. Open network adapter settings

Click:

```text
Change adapter settings
```

to directly open the Windows Network Connections panel.

### 5. Open the developer website

Click:

```text
Developer
```

to open the developer's official website.

---

## 🧱 Suggested Project Structure

```text
DNSNetworkUtility/
│
├── DNSNetworkUtility.sln
│
├── DNSNetworkUtility/
│   │
│   ├── Program.cs
│   ├── MainForm.cs
│   ├── MainForm.Designer.cs
│   ├── MainForm.resx
│   │
│   ├── NetworkActions.cs
│   ├── Models/
│   │   ├── DnsServerInfo.cs
│   │   └── DnsPingResult.cs
│   │
│   ├── Helpers/
│   │   ├── ProcessHelper.cs
│   │   └── ValidationHelper.cs
│   │
│   ├── Resources/
│   │   └── icons/
│   │
│   └── Properties/
│       ├── Resources.resx
│       └── Settings.settings
│
├── README.md
├── LICENSE
├── .gitignore
└── screenshots/
    ├── main-window.png
    └── dns-result.png
```

> The exact folder structure may vary depending on the current project implementation.

---

## 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| C# | Main programming language |
| .NET / .NET 8 | Application runtime |
| Windows Forms | Desktop user interface |
| `System.Net.NetworkInformation` | Ping requests and network responses |
| `System.Diagnostics` | Launching Windows tools and URLs |
| LINQ | Sorting and processing results |
| Visual Studio | Development environment |
| Git | Version control |

---

## 💻 Programming Language

The main programming language used in this project is:

```text
C#
```

The project focuses on:

- Object-oriented programming
- Exception handling
- Asynchronous programming
- Separation of network logic from the user interface
- Clean and maintainable code
- Simple and practical user experience

---

## ⚙️ Requirements

To run the project in development mode, you need:

- Windows operating system
- Visual Studio 2022 or newer
- The appropriate .NET SDK
- Network access for DNS testing

For `.NET 8`, installing the following runtime is recommended:

```text
.NET 8 Desktop Runtime
```

---

## 🔧 Run the Project

Clone the repository:

```bash
git clone https://github.com/USERNAME/DNSNetworkUtility.git
```

Navigate to the project directory:

```bash
cd DNSNetworkUtility
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build --configuration Release
```

Run the project:

```bash
dotnet run --project DNSNetworkUtility
```

You can also open the Solution file in Visual Studio and run the project from there.

---

## 📦 Build a Release Version

To build a standard Release version:

```bash
dotnet build --configuration Release
```

The output is usually generated inside:

```text
bin/Release/
```

---

## 📤 Publish a Self-Contained Windows Build

To publish a self-contained Windows x64 version:

```bash
dotnet publish DNSNetworkUtility.csproj \
  --configuration Release \
  --runtime win-x64 \
  --self-contained true \
  --output publish/win-x64
```

On Windows Command Prompt:

```cmd
dotnet publish DNSNetworkUtility.csproj --configuration Release --runtime win-x64 --self-contained true --output publish\win-x64
```

A self-contained build does not require the end user to install the .NET Runtime separately.

---

## 🗜️ Create a ZIP Release Package

Using PowerShell:

```powershell
Compress-Archive `
  -Path .\publish\win-x64\* `
  -DestinationPath .\publish\DNSNetworkUtility-win-x64.zip `
  -Force
```

The final release package will be created at:

```text
publish/DNSNetworkUtility-win-x64.zip
```

---

## 🔐 License

This project is distributed under the license available in the `LICENSE` file.

If no license has been selected yet, consider choosing one of the following before making the repository public:

- MIT License
- Apache License 2.0
- GNU GPL v3.0

---

## 👨‍💻 About the Developer

This project was developed by **Mohammadreza Amoori**.

Mohammadreza Amoori is:

- A computer engineering student
- An associate degree software engineering student
- Active in programming and technology
- Interested in digital technology, education, and psychology
- A volleyball athlete
- Ranked 62nd in Iran's national entrance exam

Official website:

[https://mramoori.ir](https://mramoori.ir)

---

## 📌 Development Roadmap

The following features may be added in future releases:

- Real DNS Query testing alongside ICMP Ping
- IPv6 support
- Test history storage
- DNS response-time charts
- Custom DNS management
- CSV and JSON export
- Automatic best DNS detection
- Multi-language support
- Dark mode
- Automatic update system

---

## 🤝 Contributing

To contribute:

1. Fork the repository.
2. Create a new branch.
3. Implement your changes.
4. Run the required tests.
5. Create a meaningful commit.
6. Open a Pull Request.

Example:

```bash
git checkout -b feature/add-dns-query-test
git add .
git commit -m "feat: add real DNS query testing"
git push origin feature/add-dns-query-test
```

---

## 🐛 Bug Reports

When reporting a bug, include:

- Windows version
- Application version
- Internet connection type
- VPN status
- Complete error message
- Steps to reproduce the issue
- Screenshot or log file, if available

---

## 📄 Disclaimer

This application is designed to inspect and analyze DNS server response behavior.

Ping results may be affected by network conditions, firewalls, VPNs, ISPs, and destination server policies.

The user is responsible for any network configuration changes made based on the application's results.