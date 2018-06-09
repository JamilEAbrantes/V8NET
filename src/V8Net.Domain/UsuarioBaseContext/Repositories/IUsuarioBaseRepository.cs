using System.Collections.Generic;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Queries;

namespace V8Net.Domain.UsuarioBaseContext.Repositories
{
    public interface IUsuarioBaseRepository
    {
        ///////////////////////////////////////////////////
        // Usuário ////////////////////////////////////////
        ///////////////////////////////////////////////////
        UsuarioBase UsuarioBase(int id);
        UsuarioBase UsuarioBase(string email);
        UsuarioBase UsuarioBase(string usuario, string senha);
        BuscarUsuariosBaseResumidoQueryResult BuscarPorId(int id);
        EditarUsuariosBaseQueryResult Editar(int id);
        IEnumerable<BuscarUsuariosBaseResumidoQueryResult> BuscarPorUsuario(string usuario);        
        IEnumerable<BuscarUsuariosBaseResumidoQueryResult> BuscarTodos();
        void Salvar(UsuarioBase usuario);        
        void Atualizar(UsuarioBase usuario);
        void Excluir(int id);
        bool DocumentoExistente(string documento);
        bool UsuarioExistente(string usuario);
        bool EmailExistente(string email);

        ///////////////////////////////////////////////////
        // Usuário Login //////////////////////////////////
        ///////////////////////////////////////////////////
        BuscarUsuarioSenhaTemporariaQueryResult BuscarUsuarioSenhaTemporaria(int id);
        void ConfigurarSenhaTemporaria(int idUsuario, string senha);

        ///////////////////////////////////////////////////
        // Usuário empresas ///////////////////////////////
        ///////////////////////////////////////////////////
        Empresa Empresa(int id);
        IEnumerable<BuscarUsuarioEmpresasResumidoQueryResult> BuscarEmpresasPorUsuario(int id);
        void SalvarUsuarioEmpresa(UsuarioEmpresa usuarioEmpresa);
        bool UsuarioEmpresaExistente(int idUsuario, int idEmpresa);
        bool UsuarioEmpresaExistente(int id);
        void ExcluirUsuarioEmpresa(int id);

        ///////////////////////////////////////////////////
        // Usuário atuação ////////////////////////////////
        ///////////////////////////////////////////////////
        AreaAtuacao AreaAtuacao(int id);
        IEnumerable<BuscarUsuarioAtuacoesQueryResult> BuscarAtuacoesPorUsuario(int id);
        void SalvarUsuarioAtuacao(UsuarioAreaAtuacao areaAtuacao);
        bool UsuarioAtuacaoExistente(int idUsuario, int idAreaAtuacao);
        bool UsuarioAtuacaoExistente(int id);
        void ExcluirUsuarioAtuacao(int id);

        ///////////////////////////////////////////////////
        // Usuário telas //////////////////////////////////
        ///////////////////////////////////////////////////
        Tela Tela(int id);
        EditarUsuarioTelaQueryResult EditarUsuarioTela(int id);
        UsuarioTela UsuarioTela(int id, UsuarioBase usuarioBase, Tela tela);
        IEnumerable<BuscarUsuarioTelasQueryResult> BuscarTelasPorUsuario(int id);
        void SalvarUsuarioTela(UsuarioTela usuarioTela);
        void AtualizarUsuarioTela(UsuarioTela usuarioTela);
        bool UsuarioTelaExistente(int idUsuario, int idTela);
        bool UsuarioTelaExistente(int id);
        void ExcluirUsuarioTela(int id);
    }
}
