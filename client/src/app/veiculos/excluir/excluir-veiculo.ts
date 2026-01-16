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
import { DetalhesVeiculoModel } from '../veiculo.models';
import { VeiculoService } from '../veiculo.service';

@Component({
  selector: 'app-excluir-veiculo',
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
  templateUrl: './excluir-veiculo.html',
})
export class ExcluirVeiculo {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly veiculoService = inject(VeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly veiculo$ = this.route.data.pipe(
    filter((data) => data['veiculo']),
    map((data) => data['veiculo'] as DetalhesVeiculoModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public excluir() {
    const exclusaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi excluído com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/veiculos']),
    };

    this.veiculo$
      .pipe(
        take(1),
        switchMap((veiculo) => this.veiculoService.excluir(veiculo.id)),
      )
      .subscribe(exclusaoObserver);
  }
}
