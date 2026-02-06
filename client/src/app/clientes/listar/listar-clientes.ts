import { filter, map } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SelecionarClientesModel } from '../cliente.models';

@Component({
  selector: 'app-listar-clientes',
  imports: [MatButtonModule, MatIconModule, MatCardModule, MatDividerModule, RouterLink, AsyncPipe],
  templateUrl: './listar-clientes.html',
})
export class ListarClientes {
  protected readonly route = inject(ActivatedRoute);

  protected readonly clientes$ = this.route.data.pipe(
    filter((data) => data['clientes']),
    map((data) => data['clientes'] as SelecionarClientesModel[]),
  );
}
