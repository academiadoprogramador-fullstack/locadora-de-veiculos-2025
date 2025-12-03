import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const compararConfirmacaoSenha: ValidatorFn = (
  group: AbstractControl,
): ValidationErrors | null => {
  const senha = group.get('senha')?.value as string | null;
  const confirmarSenha = group.get('confirmarSenha')?.value as string | null;

  if (senha !== confirmarSenha) return { compararConfirmacaoSenha: true };

  return null;
};
