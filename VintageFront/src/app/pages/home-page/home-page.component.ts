import { LowerCasePipe, NgFor, UpperCasePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CardEventComponent } from '../../commons/components/card-event/card-event.component';
import { PATH_BUY_PAGES } from '../../commons/config/path-pages';
import { ICardEvent } from '../../commons/models/components.interface';
import { IHomeGenres } from '../../commons/services/api/home/home-api.interface';
import { HomeApiService } from '../../commons/services/api/home/home-api.service';
import { SharedFormCompleteModule } from '../../commons/shared/shared-form-complete.module';

@Component({
	standalone: true,
	selector: 'app-home-page',
	templateUrl: './home-page.component.html',
	styleUrls: ['./home-page.component.scss'],
	imports: [SharedFormCompleteModule, CardEventComponent, UpperCasePipe, LowerCasePipe, NgFor]
})
export class HomePageComponent implements OnInit {
	listGenres: IHomeGenres[] = [];
	listConcerts: ICardEvent[] = [];
	private readonly _homeApiService = inject(HomeApiService);
	private readonly _router = inject(Router);

	ngOnInit(): void {
		this._loadHome();
	}

	clickCard(event: ICardEvent): void {
		this._router.navigate([PATH_BUY_PAGES.buyPage.withSlash], { state: { event } });
	}

	private _loadHome() {
		this._homeApiService.getHome().subscribe({
			next: (response) => {
				this.listGenres = response.tipos;
				this.listConcerts = response.getDataCardEvent();
			},
			error: (error) => {
				console.log('--error-', error);
			}
		});
	}
}
