using System.Data;

namespace QuestExampleApi.Helpers
{
    public class Helper
    {
        public static async Task<(DataTable masterTable, DataTable detayTable)> FaturaVerisiOlusturAsync()
        {
            return await Task.Run(() =>
            {
                DataTable dtMaster = new DataTable();
                dtMaster.Columns.Add("FaturaNo", typeof(string));
                dtMaster.Columns.Add("FaturaTarihi", typeof(DateTime));
                dtMaster.Columns.Add("CariAdi", typeof(string));
                dtMaster.Columns.Add("ToplamTutar", typeof(decimal));

                DataTable dtDetay = new DataTable();
                dtDetay.Columns.Add("FaturaNo", typeof(string));
                dtDetay.Columns.Add("UrunAdi", typeof(string));
                dtDetay.Columns.Add("Miktar", typeof(int));
                dtDetay.Columns.Add("BirimFiyat", typeof(decimal));
                dtDetay.Columns.Add("Tutar", typeof(decimal));

                string faturaNo = "FTR20240001";
                DataRow masterRow = dtMaster.NewRow();
                masterRow["FaturaNo"] = faturaNo;
                masterRow["FaturaTarihi"] = DateTime.Now;
                masterRow["CariAdi"] = "ABC Ticaret Ltd.";
                masterRow["ToplamTutar"] = 0;  
                dtMaster.Rows.Add(masterRow);

                decimal toplamTutar = 0;
                for (int i = 1; i <= 30; i++)
                {
                    DataRow detayRow = dtDetay.NewRow();
                    detayRow["FaturaNo"] = faturaNo;
                    detayRow["UrunAdi"] = "Ürün " + i;
                    detayRow["Miktar"] = i * 2;  
                    detayRow["BirimFiyat"] = 10 + i;  
                    detayRow["Tutar"] = (int)detayRow["Miktar"] * (decimal)detayRow["BirimFiyat"];

                    toplamTutar += (decimal)detayRow["Tutar"];

                    dtDetay.Rows.Add(detayRow);
                }

                dtMaster.Rows[0]["ToplamTutar"] = toplamTutar;

                return (dtMaster, dtDetay);
            });
        }
    }
}
