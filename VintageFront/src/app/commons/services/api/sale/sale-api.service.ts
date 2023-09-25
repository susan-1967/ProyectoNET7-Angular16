import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { IResponse } from '../api-models-base.interface';
import {
	IRequestCreateSale,
	IRequestListSalesByGenre,
	IResponseListSales,
	IResponseSale
} from './sale-api-model.interface';

const URL_SALE = environment.host + '/Venta';
const URL_LIST_SALE = URL_SALE + '/ListVentas';
const URL_LIST_SALE_BY_DATE = URL_SALE + '/ListVentasByDate';

@Injectable({ providedIn: 'root' })
export class SaleApiService {
	private _httpClient = inject(HttpClient);

	createSale(sale: IRequestCreateSale): Observable<IResponse<number>> {
		return this._httpClient.post<IResponse<number>>(URL_SALE, sale);
	}

	getSale(idSale: number): Observable<IResponse<IResponseSale>> {
		return this._httpClient.get<IResponse<IResponseSale>>(`${URL_SALE}/${idSale}`);
	}

	getSalesUser(filter: string, page?: number, rows?: number): Observable<IResponse<IResponseListSales[]>> {
		return this._httpClient.get<IResponse<IResponseListSales[]>>(URL_LIST_SALE);
	}

	getListSales(request: IRequestListSalesByGenre): Observable<IResponse<IResponseListSales[]>> {
		let params = new HttpParams().set('dateStart', request.dateStart).set('dateEnd', request.dateEnd);
		if (request.page) {
			params = params.set('page', request.page);
		}
		if (request.rows) {
			params = params.set('rows', request.rows);
		}

		return this._httpClient.get<IResponse<IResponseListSales[]>>(URL_LIST_SALE_BY_DATE, { params });
	}
}
