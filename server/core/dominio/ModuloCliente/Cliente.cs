using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class Cliente : EntidadeBase<Cliente>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }

    public TipoCliente Tipo { get; set; }
    public string NumeroDocumento { get; set; }

    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    public List<Condutor> Condutores { get; set; } = [];

    public Cliente(
        Guid empresaId,
        string nome,
        string email,
        string telefone,
        TipoCliente tipo,
        string numeroDocumento,
        string cidade,
        string estado,
        string bairro,
        string rua,
        string numero
    )
    {
        EmpresaId = empresaId;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Tipo = tipo;
        NumeroDocumento = numeroDocumento;
        Cidade = cidade;
        Estado = estado;
        Bairro = bairro;
        Rua = rua;
        Numero = numero;
    }

    public override void AtualizarRegistro(Cliente registroEditado)
    {
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone = registroEditado.Telefone;
        Tipo = registroEditado.Tipo;
        NumeroDocumento = registroEditado.NumeroDocumento;
        Cidade = registroEditado.Cidade;
        Estado = registroEditado.Estado;
        Bairro = registroEditado.Bairro;
        Rua = registroEditado.Rua;
        Numero = registroEditado.Numero;
    }
}
