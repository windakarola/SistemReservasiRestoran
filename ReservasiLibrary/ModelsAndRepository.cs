using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReservasiLibrary
{
    // ================= 1. ENTITAS TABEL 1: RESTORAN =================
    public class Restoran
    {
        public int IdRestoran { get; set; }
        public string NamaRestoran { get; set; }
        public string Alamat { get; set; }
        public string NoTelp { get; set; }
        public int KapasitasMeja { get; set; }
    }

    // ================= 2. ENTITAS TABEL 2: MENU =================
    public class MenuResto
    {
        public int IdMenu { get; set; }
        public int IdRestoran { get; set; } // Foreign Key ke Restoran
        public string NamaMenu { get; set; }
        public string KategoriPaket { get; set; } // Reguler / AYCE
        public int Harga { get; set; }
        public int Terjual { get; set; }
    }

    // ================= 3. ENTITAS TABEL 3: BOOKING =================
    public class Booking
    {
        public int IdBooking { get; set; }
        public int IdRestoran { get; set; } // Foreign Key ke Restoran
        public int IdMenu { get; set; }     // Foreign Key ke Menu
        public string NamaPelanggan { get; set; }
        public string WaktuBooking { get; set; }
        public int JumlahKursi { get; set; }
        public int TotalBayar { get; set; }
        public string StatusBooking { get; set; } // Antrean / Lunas / Dibatalkan
    }

    // ================= 4. ENTITAS TABEL 4: PEMBATALAN =================
    public class Pembatalan
    {
        public int IdCancel { get; set; }
        public int IdBooking { get; set; } // Foreign Key ke Booking
        public string NamaPelanggan { get; set; }
        public string WaktuCancel { get; set; }
        public string AlasanBatal { get; set; }
        public string AdminPenanggungJawab { get; set; }
    }

    // ================= REPOSITORY & BUSINESS LOGIC =================
    public static class DataRepository
    {
        public static List<Restoran> ListRestoran = new List<Restoran>();
        public static List<MenuResto> ListMenu = new List<MenuResto>();
        public static List<Booking> ListBooking = new List<Booking>();
        public static List<Pembatalan> ListPembatalan = new List<Pembatalan>();

        private const string FILE_RESTO = "data_restoran.txt";
        private const string FILE_MENU = "data_menu.txt";
        private const string FILE_BOOKING = "data_booking.txt";
        private const string FILE_CANCEL = "data_cancel.txt";

        public static void MuatSemuaData()
        {
            ListRestoran.Clear();
            ListMenu.Clear();
            ListBooking.Clear();
            ListPembatalan.Clear();

            if (File.Exists(FILE_RESTO))
            {
                foreach (var line in File.ReadAllLines(FILE_RESTO))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var p = line.Split('|');
                    if (p.Length >= 5)
                    {
                        ListRestoran.Add(new Restoran
                        {
                            IdRestoran = int.Parse(p[0]),
                            NamaRestoran = p[1],
                            Alamat = p[2],
                            NoTelp = p[3],
                            KapasitasMeja = int.Parse(p[4])
                        });
                    }
                }
            }

            if (File.Exists(FILE_MENU))
            {
                foreach (var line in File.ReadAllLines(FILE_MENU))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var p = line.Split('|');
                    if (p.Length >= 6)
                    {
                        ListMenu.Add(new MenuResto
                        {
                            IdMenu = int.Parse(p[0]),
                            IdRestoran = int.Parse(p[1]),
                            NamaMenu = p[2],
                            KategoriPaket = p[3],
                            Harga = int.Parse(p[4]),
                            Terjual = int.Parse(p[5])
                        });
                    }
                }
            }

            if (File.Exists(FILE_BOOKING))
            {
                foreach (var line in File.ReadAllLines(FILE_BOOKING))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var p = line.Split('|');
                    if (p.Length >= 8)
                    {
                        ListBooking.Add(new Booking
                        {
                            IdBooking = int.Parse(p[0]),
                            IdRestoran = int.Parse(p[1]),
                            IdMenu = int.Parse(p[2]),
                            NamaPelanggan = p[3],
                            WaktuBooking = p[4],
                            JumlahKursi = int.Parse(p[5]),
                            TotalBayar = int.Parse(p[6]),
                            StatusBooking = p[7]
                        });
                    }
                }
            }

            if (File.Exists(FILE_CANCEL))
            {
                foreach (var line in File.ReadAllLines(FILE_CANCEL))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var p = line.Split('|');
                    if (p.Length >= 6)
                    {
                        ListPembatalan.Add(new Pembatalan
                        {
                            IdCancel = int.Parse(p[0]),
                            IdBooking = int.Parse(p[1]),
                            NamaPelanggan = p[2],
                            WaktuCancel = p[3],
                            AlasanBatal = p[4],
                            AdminPenanggungJawab = p[5]
                        });
                    }
                }
            }

            // Jika belum ada data sama sekali, isi data awal bawaan seperti di C++
            if (ListRestoran.Count == 0)
            {
                InisialisasiDataAwal();
                SimpanSemuaData();
            }
        }

        public static void SimpanSemuaData()
        {
            File.WriteAllLines(FILE_RESTO, ListRestoran.Select(r =>
                $"{r.IdRestoran}|{r.NamaRestoran}|{r.Alamat}|{r.NoTelp}|{r.KapasitasMeja}"));

            File.WriteAllLines(FILE_MENU, ListMenu.Select(m =>
                $"{m.IdMenu}|{m.IdRestoran}|{m.NamaMenu}|{m.KategoriPaket}|{m.Harga}|{m.Terjual}"));

            File.WriteAllLines(FILE_BOOKING, ListBooking.Select(b =>
                $"{b.IdBooking}|{b.IdRestoran}|{b.IdMenu}|{b.NamaPelanggan}|{b.WaktuBooking}|{b.JumlahKursi}|{b.TotalBayar}|{b.StatusBooking}"));

            File.WriteAllLines(FILE_CANCEL, ListPembatalan.Select(c =>
                $"{c.IdCancel}|{c.IdBooking}|{c.NamaPelanggan}|{c.WaktuCancel}|{c.AlasanBatal}|{c.AdminPenanggungJawab}"));
        }

        private static void InisialisasiDataAwal()
        {
            ListRestoran.Add(new Restoran { IdRestoran = 1, NamaRestoran = "Resto Padang Minang", Alamat = "Jl. Kaliurang KM 5, Sleman", NoTelp = "081234567801", KapasitasMeja = 25 });
            ListRestoran.Add(new Restoran { IdRestoran = 2, NamaRestoran = "Shabu-Shabu Grill", Alamat = "Jl. Ringroad Utara, Depok", NoTelp = "081234567802", KapasitasMeja = 40 });

            ListMenu.Add(new MenuResto { IdMenu = 1, IdRestoran = 1, NamaMenu = "Rendang Daging", KategoriPaket = "Reguler", Harga = 25000, Terjual = 150 });
            ListMenu.Add(new MenuResto { IdMenu = 2, IdRestoran = 1, NamaMenu = "Ayam Pop", KategoriPaket = "Reguler", Harga = 22000, Terjual = 90 });
            ListMenu.Add(new MenuResto { IdMenu = 3, IdRestoran = 1, NamaMenu = "Gulai Tunjang", KategoriPaket = "Reguler", Harga = 27000, Terjual = 200 });

            ListMenu.Add(new MenuResto { IdMenu = 4, IdRestoran = 2, NamaMenu = "Paket AYCE Premium", KategoriPaket = "AYCE", Harga = 150000, Terjual = 80 });
            ListMenu.Add(new MenuResto { IdMenu = 5, IdRestoran = 2, NamaMenu = "Paket AYCE Regular", KategoriPaket = "AYCE", Harga = 99000, Terjual = 120 });
            ListMenu.Add(new MenuResto { IdMenu = 6, IdRestoran = 2, NamaMenu = "Wagyu Plate", KategoriPaket = "Reguler", Harga = 50000, Terjual = 45 });

            ListBooking.Add(new Booking { IdBooking = 101, IdRestoran = 1, IdMenu = 3, NamaPelanggan = "Tania", WaktuBooking = "19:00", JumlahKursi = 2, TotalBayar = 54000, StatusBooking = "Antrean" });
            ListBooking.Add(new Booking { IdBooking = 102, IdRestoran = 2, IdMenu = 5, NamaPelanggan = "Haidar", WaktuBooking = "20:00", JumlahKursi = 4, TotalBayar = 396000, StatusBooking = "Antrean" });
        }

        // Helper mencari nama dari Foreign Key
        public static string GetNamaRestoran(int idResto)
        {
            var r = ListRestoran.FirstOrDefault(x => x.IdRestoran == idResto);
            return r != null ? r.NamaRestoran : "-";
        }

        public static string GetNamaMenu(int idMenu)
        {
            var m = ListMenu.FirstOrDefault(x => x.IdMenu == idMenu);
            return m != null ? m.NamaMenu : "-";
        }

        // Bubble Sort Descending untuk Menu Best Seller (konversi dari C++)
        public static List<MenuResto> UrutkanMenuBestSeller(List<MenuResto> sumber)
        {
            List<MenuResto> arr = new List<MenuResto>(sumber);
            int n = arr.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j].Terjual < arr[j + 1].Terjual)
                    {
                        var temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            return arr;
        }
    }
}