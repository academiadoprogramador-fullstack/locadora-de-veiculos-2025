using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Dominio.ModuloCondutor;

public class Condutor : EntidadeBase<Condutor>
{
    public Guid ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public bool ClienteCondutor { get; set; }

    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cpf { get; set; }
    public string Cnh { get; set; }
    public DateTimeOffset ValidadeCnh { get; set; }

    public Condutor(
        Guid empresaId,
        Guid clienteId,
        bool clienteCondutor,
        string nome,
        string email,
        string telefone,
        string cpf,
        string cnh,
        DateTimeOffset validadeCnh
    )
    {
        EmpresaId = empresaId;
        ClienteId = clienteId;
        ClienteCondutor = clienteCondutor;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cpf = cpf;
        Cnh = cnh;
        ValidadeCnh = validadeCnh;
    }

    public override void AtualizarRegistro(Condutor registroEditado)
    {
        ClienteId = registroEditado.ClienteId;
        ClienteCondutor = registroEditado.ClienteCondutor;
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone = registroEditado.Telefone;
        Cpf = registroEditado.Cpf;
        Cnh = registroEditado.Cnh;
        ValidadeCnh = registroEditado.ValidadeCnh;
    }
}
