import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-inicio',
  imports: [MatCardModule, MatButtonModule, MatIconModule, MatDividerModule, MatTooltipModule],
  templateUrl: './inicio.html',
})
export class Inicio {}
