import { filter, map, shareReplay } from 'rxjs';

import { AsyncPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { DetalhesAluguelModel } from '../aluguel.model';

@Component({
  selector: 'app-detalhes-aluguel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    RouterLink,
    AsyncPipe,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './detalhes-aluguel.html',
})
export class DetalhesAluguel {
  protected readonly route = inject(ActivatedRoute);

  protected readonly aluguel$ = this.route.data.pipe(
    filter((data) => data['aluguel']),
    map((data) => data['aluguel'] as DetalhesAluguelModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );
}
