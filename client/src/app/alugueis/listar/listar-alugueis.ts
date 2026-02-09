import { filter, map } from 'rxjs';

import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SelecionarAlugueisModel, StatusAluguel } from '../aluguel.model';

@Component({
  selector: 'app-listar-alugueis',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDividerModule,
    RouterLink,
    AsyncPipe,
    DatePipe,
  ],
  templateUrl: './listar-alugueis.html',
})
export class ListarAlugueis {
  protected readonly route = inject(ActivatedRoute);

  protected readonly alugueis$ = this.route.data.pipe(
    filter((data) => data['alugueis']),
    map((data) => data['alugueis'] as SelecionarAlugueisModel[]),
  );

  protected corStatusFormatado(status: StatusAluguel) {
    return status === 'Concluido'
      ? 'bg-emerald-50 text-emerald-700 ring-1 ring-emerald-200'
      : 'bg-amber-50 text-amber-700 ring-1 ring-amber-200';
  }

  protected textoStatusFormatado(status: StatusAluguel) {
    switch (status) {
      case 'Simulacao':
        return 'Simulação';
      case 'Aberto':
        return 'Aberto';
      case 'Concluido':
        return 'Concluído';

      default:
        return 'Status Inconclusivo';
    }
  }
}
