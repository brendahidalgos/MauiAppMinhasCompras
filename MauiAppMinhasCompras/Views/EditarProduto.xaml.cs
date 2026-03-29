using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Produto produto)
        {
            dtp_data_compra.Date = produto.DataCadastro == DateTime.MinValue
                ? DateTime.Today
                : produto.DataCadastro;
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto? produto_anexado = BindingContext as Produto;
            if (produto_anexado == null)
            {
                await DisplayAlert("Ops", "Nenhum produto foi selecionado para edicao.", "OK");
                return;
            }

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_qtd.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                DataCadastro = dtp_data_compra.Date
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso", "Resgistro Atualizado", "ok");
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
