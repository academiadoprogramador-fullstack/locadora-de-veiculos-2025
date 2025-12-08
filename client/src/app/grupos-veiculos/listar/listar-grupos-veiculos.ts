import { filter, map } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SelecionarGruposVeiculosModel } from '../grupo-veiculos.models';

@Component({
  selector: 'app-listar-grupos-veiculos',
  imports: [MatButtonModule, MatIconModule, MatCardModule, RouterLink, AsyncPipe],
  templateUrl: './listar-grupos-veiculos.html',
})
export class ListarGruposVeiculos {
  protected readonly route = inject(ActivatedRoute);

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as SelecionarGruposVeiculosModel[]),
  );
}
