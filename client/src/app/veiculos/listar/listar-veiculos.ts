import { filter, map } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SelecionarVeiculosModel } from '../veiculo.models';

@Component({
  selector: 'app-listar-veiculos',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDividerModule,
    RouterLink,
    AsyncPipe,
  ],
  templateUrl: './listar-veiculos.html',
})
export class ListarVeiculos {
  protected readonly route = inject(ActivatedRoute);

  protected readonly urlPublicoS3 = 'https://pub-ed252ae2497a49449f8c2159a5331e5f.r2.dev/';

  protected readonly veiculos$ = this.route.data.pipe(
    filter((data) => data['veiculos']),
    map((data) => data['veiculos'] as SelecionarVeiculosModel[]),
  );
}
