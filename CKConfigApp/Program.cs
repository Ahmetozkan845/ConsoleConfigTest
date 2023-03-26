using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        //App.config value içerisinde proje yolunun doğru verildiğinden emin olunmalı
        try
        {
            //ConfigurationManager sınıfı makine, uygulama ve kullanıcı yapılandırma bilgilerine erişmenizi sağlar. 
            string appPath = ConfigurationManager.AppSettings["AppPath"];
            //Path dolu olup veya boş olduğunu kontrol eden yapı
            if (string.IsNullOrEmpty(appPath) || !File.Exists(appPath))
            {
                Console.WriteLine("Uygulama yolu yanlış veya boş.");
                return;
            }

            Console.WriteLine("Uygulama yolu: " + appPath);
            //Process sınıfı için bir özelliktir ve bir uygulamanın nasıl başlatılacağı hakkındaki bilgileri tutar
            //RedirectStandardOutput özelliğinin true olarak ayarlanması, başlatılan uygulamanın çıktısını process.
            //StandardOutput özelliğiyle almak için gereklidir.
            //Ancak, UseShellExecute özelliği false olarak ayarlandığında kullanıcı arayüzü işleminin kullanılamaz
            ProcessStartInfo startInfo = new ProcessStartInfo(appPath);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            //Proses kontrolü 
            Process process = new Process();
            process.StartInfo = startInfo;

            if (!process.Start())
            {
                Console.WriteLine("Uygulama başlatılamadı.");
                return;
            }

            Console.WriteLine("Uygulama başarıyla başlatıldı.");
            Console.WriteLine(process.StandardOutput.ReadToEnd());
        }
        //Log Yapısı
        catch (Exception ex)
        {
            string logFilePath = "log.txt";
            string errorMessage = string.Format("{0} Tarihinde Hata Oluştu: {1}\n\n", DateTime.Now, ex.Message);

            if (!File.Exists(logFilePath))
            {
                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                    sw.WriteLine(errorMessage);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(errorMessage);
                }
            }

            Console.WriteLine("Hata. Lütfen log dosyasını kontrol edin.");
           
        }
    }
}
