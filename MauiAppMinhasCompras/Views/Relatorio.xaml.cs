using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class Relatorio : ContentPage
{
    readonly ObservableCollection<Produto> lista = new();

    public Relatorio()
    {
        InitializeComponent();
        stack_produtos.BindingContext = lista;

        dtp_data_final.Date = DateTime.Today;
        dtp_data_inicial.Date = DateTime.Today.AddMonths(-1);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (dtp_data_inicial.Date > dtp_data_final.Date)
            {
                await DisplayAlert("Ops", "A data inicial deve ser menor ou igual a data final.", "OK");
                return;
            }

            List<Produto> produtos = await App.Db.GetByPeriod(dtp_data_inicial.Date, dtp_data_final.Date);

            lista.Clear();
            produtos.ForEach(lista.Add);
            lbl_vazio.IsVisible = produtos.Count == 0;

            double totalPeriodo = produtos.Sum(p => p.Total);
            lbl_resumo.Text = $"Periodo: {dtp_data_inicial.Date:dd/MM/yyyy} a {dtp_data_final.Date:dd/MM/yyyy} | Itens: {produtos.Count} | Total: {totalPeriodo:C}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
