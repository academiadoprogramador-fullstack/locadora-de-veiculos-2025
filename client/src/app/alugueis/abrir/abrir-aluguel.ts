import { Observer } from 'rxjs';

import { CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { SimularAberturaAluguelResponseModel } from '../aluguel.model';
import { AluguelService } from '../aluguel.service';

@Component({
  selector: 'app-abrir-aluguel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    RouterLink,
    CurrencyPipe,
  ],
  templateUrl: './abrir-aluguel.html',
})
export class AbrirAluguel {
  private readonly router = inject(Router);
  private readonly aluguelService = inject(AluguelService);
  private readonly notificacaoService = inject(NotificacaoService);

  protected readonly aluguelSimulado: SimularAberturaAluguelResponseModel | null =
    (this.router.currentNavigation()?.extras.state?.['aluguelSimulado'] as
      | SimularAberturaAluguelResponseModel
      | undefined) ??
    (history.state?.['aluguelSimulado'] as SimularAberturaAluguelResponseModel | undefined) ??
    null;

  constructor() {
    if (!this.aluguelSimulado?.id) {
      this.notificacaoService.erro(
        'Não foi possível carregar a simulação do aluguel. Faça o cadastro novamente.',
      );

      this.router.navigate(['/alugueis', 'cadastrar']);
    }
  }

  public confirmarAbertura() {
    if (!this.aluguelSimulado?.id) return;

    const abrirObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso('Aluguel aberto com sucesso!'),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/alugueis']),
    };

    this.aluguelService.abrir(this.aluguelSimulado.id).subscribe(abrirObserver);
  }
}
