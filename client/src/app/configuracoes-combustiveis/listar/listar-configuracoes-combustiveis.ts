import { filter, map } from 'rxjs';

import { AsyncPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SelecionarConfiguracoesCombustiveisModel } from '../configuracao-combustiveis.models';

@Component({
  selector: 'app-listar-configuracoes-combustiveis',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    AsyncPipe,
    CurrencyPipe,
    DatePipe,
    RouterLink,
  ],
  templateUrl: './listar-configuracoes-combustiveis.html',
})
export class ListarConfiguracoesCombustiveis {
  protected readonly route = inject(ActivatedRoute);

  protected readonly configuracoes$ = this.route.data.pipe(
    filter((data) => data['configuracoes']),
    map((data) => data['configuracoes'] as SelecionarConfiguracoesCombustiveisModel[]),
  );

  protected readonly displayedColumns = [
    'criadaEm',
    'valorGasolina',
    'valorAlcool',
    'valorDiesel',
    'valorGas',
    'valorEletricidade',
  ] as const;
}
