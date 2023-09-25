import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class DemoService {
	saludo = '';
	constructor() {
		console.log('-----DemoService----');
	}

	getSaludo() {
		return `Hola, ${this.saludo}`;
	}
}
