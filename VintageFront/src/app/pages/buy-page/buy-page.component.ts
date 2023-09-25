import { CurrencyPipe, DatePipe, NgSwitch, NgSwitchCase, NgSwitchDefault } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastEvokeService } from '@costlydeveloper/ngx-awesome-popup';
import { mergeMap } from 'rxjs';
import { CardEventComponent } from '../../commons/components/card-event/card-event.component';
import { PATHS_AUTH_PAGES } from '../../commons/config/path-pages';
import { ICardEvent } from '../../commons/models/components.interface';
import { CustomCurrencyPipe } from '../../commons/pipes/custom-currency.pipe';
import { IRequestCreateSale, IResponseSale } from '../../commons/services/api/sale/sale-api-model.interface';
import { SaleApiService } from '../../commons/services/api/sale/sale-api.service';
import { DataUserService } from '../../commons/services/local/data-user.service';
import { SharedFormCompleteModule } from '../../commons/shared/shared-form-complete.module';

type StatusBuy = 'INFO' | 'BUY' | 'VOUCHER';

@Component({
	standalone: true,
	selector: 'app-buy-page',
	templateUrl: './buy-page.component.html',
	styleUrls: ['./buy-page.component.scss'],
	imports: [
		SharedFormCompleteModule,
		CardEventComponent,
		CustomCurrencyPipe,
		CurrencyPipe,
		DatePipe,
		NgSwitchDefault,
		NgSwitchCase,
		NgSwitch,
		FormsModule
	]
})
export default class BuyPageComponent {
	statusBuy: StatusBuy = 'INFO';
	dateDemo = new Date();
	cardEvent?: ICardEvent;
	voucher?: IResponseSale;

	numberEntries = 0;
	total = 0;

	private _router = inject(Router);
	private _saleApiService = inject(SaleApiService);
	private _toastEvokeService = inject(ToastEvokeService);
	private _dataUserService = inject(DataUserService);

	constructor() {
		this._captureData();
	}

	clickBuy(statusBuy: StatusBuy): void {
		console.log(this._dataUserService.isExpiredToken())
		if (this._dataUserService.isExpiredToken()) {
			this._router.navigateByUrl(PATHS_AUTH_PAGES.loginPage.withSlash);
			return;
		}

		if (statusBuy === 'VOUCHER') {
			this._saveBuy();
			return;
		}

		this.statusBuy = statusBuy;
	}

	private _saveBuy(): void {
		const sendBuy: IRequestCreateSale = {
			productoId: this.cardEvent!.idEvent,
			cantidad: this.numberEntries
		};

		this._saleApiService
			.createSale(sendBuy)
			.pipe(
				mergeMap((response) => {
					return this._saleApiService.getSale(response.data);
				})
			)
			.subscribe((voucher) => {
				if (voucher && voucher.success) {
					this.voucher = voucher.data;
					this.statusBuy = 'VOUCHER';
					this._toastEvokeService.success('Compra', 'Su compra se ha realizado con exito, gracias.');
				}
			});
	}

	inputCalculate(): void {
		this.total = this.numberEntries * this.cardEvent!.price;
	}
	private _captureData(): void {
		// capturamos los datos del "concierto" seleccionado enviados por la opción "state"
		const navigation = this._router.getCurrentNavigation();

		if (navigation?.extras?.state && navigation.extras.state['event']) {
			this.cardEvent = navigation.extras.state['event'] as ICardEvent;
		}

		if (!this.cardEvent) {
			this._router.navigateByUrl('/');
		}
	}
}
