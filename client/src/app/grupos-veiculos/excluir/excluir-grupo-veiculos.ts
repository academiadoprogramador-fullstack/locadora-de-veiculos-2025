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
import { DetalhesGrupoVeiculosModel } from '../grupo-veiculos.models';
import { GrupoVeiculosService } from '../grupo-veiculos.service';

@Component({
  selector: 'app-excluir-grupo-veiculos',
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
  templateUrl: './excluir-grupo-veiculos.html',
})
export class ExcluirGrupoVeiculos {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly grupoVeiculosService = inject(GrupoVeiculosService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly grupoVeiculos$ = this.route.data.pipe(
    filter((data) => data['grupoVeiculos']),
    map((data) => data['grupoVeiculos'] as DetalhesGrupoVeiculosModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public excluir() {
    const exclusaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi excluído com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/grupos-veiculos']),
    };

    this.grupoVeiculos$
      .pipe(
        take(1),
        switchMap((grupoVeiculos) => this.grupoVeiculosService.excluir(grupoVeiculos.id)),
      )
      .subscribe(exclusaoObserver);
  }
}
