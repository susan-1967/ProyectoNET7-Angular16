import { Component } from '@angular/core';

@Component({
	selector: 'app-root',
	template: `<app-container />
		<ngx-ui-loader
			fgsColor="#e91e63"
			fgsType="rectangle-bounce-pulse-out"
			[fgsSize]="80"
			pbColor="#e91e63"
		></ngx-ui-loader>`
})
export class AppComponent {}
