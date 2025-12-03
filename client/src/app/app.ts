import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  template: `
    <main class="container-fluid py-3">
      <router-outlet></router-outlet>
    </main>
  `,
})
export class App {}
