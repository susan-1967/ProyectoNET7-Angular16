import {
	HttpErrorResponse,
	HttpEvent,
	HttpHandler,
	HttpHandlerFn,
	HttpInterceptor,
	HttpRequest
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ToastEvokeService } from '@costlydeveloper/ngx-awesome-popup';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Observable, catchError, finalize, throwError } from 'rxjs';

@Injectable()
export class ErrorApiInterceptor implements HttpInterceptor {
	private _ngxUiLoaderService = inject(NgxUiLoaderService);
	private _toastEvokeService = inject(ToastEvokeService);

	intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		this._ngxUiLoaderService.start();

		return next.handle(req).pipe(
			finalize(() => this._ngxUiLoaderService.stop()),
			catchError((error: HttpErrorResponse) => {
				this.errorsHttpClient(error);
				return throwError(() => error);
			})
		);
	}

	private errorsHttpClient(httpErrorResponse: HttpErrorResponse): void {
		switch (httpErrorResponse.status) {
			case 0:
			case 500:
			case 400:
			case 401:
				this._toastEvokeService.danger('Error', 'Ups,ocurrio un error inesperado, intenta nuevamente.');
				break;
			case 404:
				this._toastEvokeService.danger('Error', 'No pudimos encontrar tu solicitud, intenta más tarde.');
				break;
		}
	}
}

export const NewErrorApiInterceptor = (request: HttpRequest<unknown>, next: HttpHandlerFn) => {
	const ngxService = inject(NgxUiLoaderService);

	ngxService.start();
	return next(request).pipe(
		finalize(() => ngxService.stop()),
		catchError((error: HttpErrorResponse) => {
			errorsHttpClient(error);
			return throwError(() => error);
		})
	);
};

//#region NUEVO ENFOQUE PARA USAR INTERCEPTORES

const errorsHttpClient = (httpErrorResponse: HttpErrorResponse): void => {
	const toastEvokeService = inject(ToastEvokeService);

	switch (httpErrorResponse.status) {
		case 0:
		case 500:
		case 400:
		case 401:
			toastEvokeService!.danger('Error', 'Ups,ocurrio un error inesperado, intenta nuevamente.');
			break;
		case 404:
			toastEvokeService!.danger('Error', 'No pudimos encontrar lo solicitado, intenta más tarde.');
			break;
	}
};
//#endregion
