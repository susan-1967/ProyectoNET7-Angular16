import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CardMenusComponent } from '../../commons/components/card-menus/card-menus.component';
import { PATH_MAINTENANCE_PAGES } from '../../commons/config/path-pages';
import { ICardMenu } from '../../commons/models/components.interface';
import { SaleApiService } from '../../commons/services/api/sale/sale-api.service';
import { ProductoApiService, TipoApiService, ReportsApiService } from './services/service-index';

@Component({
	standalone: true,
	selector: 'app-maintenance',
	templateUrl: './maintenance.component.html',
	styleUrls: ['./maintenance.component.scss'],
	imports: [CardMenusComponent, RouterOutlet],
	providers: [ProductoApiService, TipoApiService, ReportsApiService, SaleApiService]
})
export class MaintenanceComponent {
	readonly menuAdmin: ICardMenu[] = [
		{
			title: 'VENTAS',
			nameImage: 'buys.png',
			path: PATH_MAINTENANCE_PAGES.buy.withSlash
		},
		{
			title: 'PRODUCTOS',
			nameImage: 'product.png',
			path: PATH_MAINTENANCE_PAGES.events.withSlash
		},
		{
			title: 'REPORTES',
			nameImage: 'statistics.png',
			path: PATH_MAINTENANCE_PAGES.reports.withSlash
		}
	];
}
