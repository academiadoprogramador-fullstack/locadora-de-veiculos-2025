import { filter, map, Observer, shareReplay, switchMap, take } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { DetalhesPlanoCobrancaModel } from '../plano-cobranca.models';
import { PlanoCobrancaService } from '../plano-cobranca.service';

@Component({
  selector: 'app-excluir-plano-cobranca',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    AsyncPipe,
    FormsModule,
  ],
  templateUrl: './excluir-plano-cobranca.html',
})
export class ExcluirPlanoCobranca {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly planoCobranca$ = this.route.data.pipe(
    filter((data) => data['planoCobranca']),
    map((data) => data['planoCobranca'] as DetalhesPlanoCobrancaModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public excluir() {
    const exclusaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi excluído com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/planos-cobranca']),
    };

    this.planoCobranca$
      .pipe(
        take(1),
        switchMap((planoCobranca) => this.planoCobrancaService.excluir(planoCobranca.id)),
      )
      .subscribe(exclusaoObserver);
  }
}
