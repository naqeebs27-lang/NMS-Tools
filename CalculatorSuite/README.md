# Calculator Suite – Native C# WinForms Conversion

This project is a native Windows Forms conversion of the supplied `Calculators Code.docx` HTML/CSS/JavaScript tools.

## Included tools
- Age Calculator
- Pakistani Salaried Person Tax Calculator
- Pakistan FBR Business Tax Calculator
- Loan EMI Calculator with year-wise amortization
- Code 39 Barcode Generator
- PDF417 Barcode Generator
- PDF to Image Converter

## Technology
- C#
- .NET 10 for Windows
- Windows Forms
- ZXing.Net for barcodes
- PdfiumViewer for PDF rendering

## Open in Visual Studio Community 2026
1. Install the **.NET desktop development** workload.
2. Open `CalculatorSuite.sln`.
3. Allow NuGet package restore.
4. Select Debug or Release.
5. Press F5.

The application is designed for normal offline use after dependencies are restored/published. The first build needs NuGet package availability.

## Important
The source document contained browser libraries (JsBarcode, bwip-js, pdf.js, Chart.js and html2pdf.js). These have been replaced by native C# implementations or .NET-compatible libraries rather than embedding the original HTML page in a browser control.

## Publish
Use Visual Studio: Build → Publish → Folder. Recommended target: `win-x64`, self-contained for a machine without .NET installed.

## Installer
For Visual Studio Community, install the Microsoft Visual Studio Installer Projects extension and create a Setup Project using the published output.
