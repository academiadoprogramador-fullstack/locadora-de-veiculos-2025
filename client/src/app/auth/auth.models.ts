export interface RegistroModel {
  nomeCompleto: string;
  email: string;
  senha: string;
  confirmarSenha: string;
}

export interface LoginModel {
  email: string;
  senha: string;
}

export interface AccessTokenModel {
  chave: string;
  expiracao: string;
  usuarioAutenticado: UsuarioAutenticadoModel;
}

export interface UsuarioAutenticadoModel {
  id: string;
  nomeCompleto: string;
  email: string;
  cargo: string;
}
