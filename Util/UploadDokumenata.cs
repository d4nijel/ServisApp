using Microsoft.AspNetCore.Http;
using System.IO;

namespace ServisApp.Util
{
    public static class UploadDokumenata
    {
        public enum TipoviDokumenata
        {
            Izvjestaji,
            Ponude,
            Ugovori,
            RadniNalozi,
            Dokumenti
        }
        public static string UploadDoc(IFormFile dokument, string brojTipDokumenta, TipoviDokumenata tipDokumenta)
        {
            var ext = Path.GetExtension(dokument.FileName); //ekstenzija .pdf

            string podFolderDokumenti, fullFileName, path, pathDB;

            if (tipDokumenta == TipoviDokumenata.Dokumenti)
            {
                podFolderDokumenti = brojTipDokumenta switch
                {
                    "Zakon" => "Zakoni",
                    "Pravilnik" => "Pravilnici",
                    "Ostalo" => "Ostalo",
                    _ => "Razno",
                };

                //path1
                //string folderDokumenti = string.Format("PDFFiles\\Dokumenti\\{0}", podFolderDokumenti); //PDFFiles\Dokumenti\Zakoni
                string folderDokumenti = string.Format("PDFFiles" + Path.AltDirectorySeparatorChar + "Dokumenti" + Path.AltDirectorySeparatorChar + "{0}", podFolderDokumenti); //PDFFiles\Dokumenti\Zakoni

                var pathDokumenti = Path.Combine(Directory.GetCurrentDirectory(), folderDokumenti);

                int fCount = 0;
                if (Directory.Exists(pathDokumenti))
                {
                    fCount = Directory.GetFiles(pathDokumenti, "*.pdf", SearchOption.TopDirectoryOnly).Length; //broj .pdf dokumenata u traženom folderu
                }

                fullFileName = brojTipDokumenta + "-" + (++fCount).ToString() + ext; //Zakon-1.pdf

                //path2
                //string folder = string.Format("PDFFiles\\{0}\\{1}", tipDokumenta, podFolderDokumenti);//PDFFiles\Dokumenti\Zakoni\
                string folder = string.Format("PDFFiles" + Path.AltDirectorySeparatorChar + "{0}" + Path.AltDirectorySeparatorChar + "{1}", tipDokumenta, podFolderDokumenti);//PDFFiles\Dokumenti\Zakoni\
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                pathDB = Path.Combine(folder, fullFileName);
                path = Path.Combine(Directory.GetCurrentDirectory(), folder, fullFileName); //PDFFiles\Zakoni\Zakon-1.pdf
            }
            else
            {
                string f1, f2;

                if (tipDokumenta == TipoviDokumenata.RadniNalozi)
                {
                    fullFileName = brojTipDokumenta.Replace("/", "-") + ext; //001-06/20 u 001-06-20.pdf
                    f1 = "20" + fullFileName.Substring(7, 2); //godina 2020
                    f2 = fullFileName.Substring(4, 2); //mjesec 06
                }
                else
                {
                    fullFileName = brojTipDokumenta + ext; //0001-06-20.pdf
                    f1 = "20" + fullFileName.Substring(8, 2); //godina 2020
                    f2 = fullFileName.Substring(5, 2); //mjesec 06
                }

                //path3
                //string folder = string.Format("PDFFiles\\{0}\\{1}\\{2}", tipDokumenta, f1, f2);//PDFFiles\Tip_dokumenta\2020\06\
                string folder = string.Format("PDFFiles" + Path.AltDirectorySeparatorChar + "{0}" + Path.AltDirectorySeparatorChar + "{1}" + Path.AltDirectorySeparatorChar + "{2}", tipDokumenta, f1, f2);//PDFFiles\Tip_dokumenta\2020\06\
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                pathDB = Path.Combine(folder, fullFileName);
                path = Path.Combine(Directory.GetCurrentDirectory(), folder, fullFileName); //PDFFiles\Tip_dokumenta\2020\06\0001-06-20.pdf
            }


            if (!File.Exists(path))
            {
                using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                dokument.CopyTo(stream);
            }

            return pathDB;
        }

        public static void DeleteDoc(string dokumentPath, bool dokument = false)
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), dokumentPath)))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), dokumentPath));
            }

            if (!dokument)
            {
                var folder = Path.GetDirectoryName(dokumentPath);//PDFFiles\RadniNalozi\2001\01\

                if (Directory.Exists(folder) && Directory.GetFiles(folder).Length == 0)
                {
                    Directory.Delete(folder);

                    var parent = Directory.GetParent(folder).FullName; //PDFFiles\RadniNalozi\2001\

                    if (Directory.Exists(parent) && Directory.GetDirectories(parent).Length == 0 && Directory.GetFiles(parent).Length == 0)
                    {
                        Directory.Delete(parent);
                    }
                }
            }
        }
    }
}
