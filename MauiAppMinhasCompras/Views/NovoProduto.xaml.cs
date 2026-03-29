using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
		dtp_data_compra.Date = DateTime.Today;
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_qtd.Text),
				Preco = Convert.ToDouble(txt_preco.Text),
				DataCadastro = dtp_data_compra.Date
			};
			await App.Db.Insert(p);
			await DisplayAlert("Sucesso", "Resgistro Inserido", "ok");
			await Navigation.PopAsync();

		}catch(Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
    }
}
