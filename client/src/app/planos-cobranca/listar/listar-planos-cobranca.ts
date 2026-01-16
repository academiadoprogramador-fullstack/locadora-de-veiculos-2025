import { filter, map } from 'rxjs';

import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SelecionarPlanosCobrancaModel } from '../plano-cobranca.models';

@Component({
  selector: 'app-listar-planos-cobranca',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDividerModule,
    RouterLink,
    AsyncPipe,
    CurrencyPipe,
  ],
  templateUrl: './listar-planos-cobranca.html',
})
export class ListarPlanosCobranca {
  protected readonly route = inject(ActivatedRoute);

  protected readonly planosCobranca$ = this.route.data.pipe(
    filter((data) => data['planosCobranca']),
    map((data) => data['planosCobranca'] as SelecionarPlanosCobrancaModel[]),
  );
}
