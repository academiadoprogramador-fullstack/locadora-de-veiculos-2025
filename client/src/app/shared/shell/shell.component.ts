import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive } from '@angular/router';

import { UsuarioAutenticadoModel } from '../../auth/auth.models';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatMenuModule,
    AsyncPipe,
    RouterLink,
    RouterLinkActive,
  ],
})
export class ShellComponent {
  private readonly breakpointObserver = inject(BreakpointObserver);

  public isHandset$: Observable<boolean> = this.breakpointObserver
    .observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Handset])
    .pipe(
      map((result) => result.matches),
      shareReplay(),
    );

  public itensNavbar = [
    { titulo: 'Início', icone: 'home', link: '/inicio' },
    { titulo: 'Grupos de Veículos', icone: 'label', link: '/grupos-veiculos' },
    { titulo: 'Planos de Cobrança', icone: 'wallet', link: '/planos-cobranca' },
    { titulo: 'Veículos', icone: 'directions_car', link: '/veiculos' },
    { titulo: 'Clientes', icone: 'person', link: '/clientes' },
    { titulo: 'Condutores', icone: 'contact_phone', link: '/condutores' },
    { titulo: 'Taxas', icone: 'paid', link: '/taxas' },
  ];

  @Input({ required: true }) usuarioAutenticado?: UsuarioAutenticadoModel;
  @Output() logoutRequisitado = new EventEmitter<void>();
}
