import { DatePipe } from '@angular/common';
import { Injectable, inject } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ConfirmBoxEvokeService, ToastEvokeService } from '@costlydeveloper/ngx-awesome-popup';
import { EMPTY, Observable, Subject, concatMap, map, of } from 'rxjs';
import { CRUD_METHOD, STATUS_CRUD } from '../../../commons/models/enums';
import { IResponse } from '../../../commons/services/api/api-models-base.interface';
import { IRequestCreateUpdateConcert, IResponseConcert } from '../services/producto/producto-api-model.interface';
import { ProductoApiService } from '../services/service-index';

@Injectable({ providedIn: 'root' })
export class EventsService {
	private readonly _formBuilder = inject(FormBuilder);
	private readonly _eventApiService = inject(ProductoApiService);
	private readonly _datePipe = inject(DatePipe);
	private readonly _confirmBoxEvokeService = inject(ConfirmBoxEvokeService);
	private readonly _toastEvokeService = inject(ToastEvokeService);

	private readonly crudSource = new Subject<boolean>();
	channelCrudEvent$ = this.crudSource.asObservable();

	crudMethod = CRUD_METHOD.SAVE;

	eventsFormGroup = this._formBuilder.nonNullable.group({
		id: [0, Validators.required],
		nombre: ['', Validators.required],
		description: ['', Validators.required],
		date: [new Date(), Validators.required],
		ticketsQuantity: [0, Validators.required],
		price: [0, Validators.required],
		status: [0, Validators.required],
		genre: this._formBuilder.control<number | null>(null),
		image: ['', Validators.required],
		fileName: ['', Validators.required]
	});
    //hour: ['', Validators.required],
    //place: ['', Validators.required],
	deleteEvent(idEvent: number): Observable<boolean> {
		return this._confirmBoxEvokeService.warning('Evento', '¿Esta seguro de eliminar el Evento?', 'Si', 'Cancelar').pipe(
			concatMap((responseQuestion) =>
				responseQuestion.success ? this._eventApiService.deleteProducto(idEvent) : EMPTY
			),
			concatMap((response) => {
				if (response.success) {
					this._toastEvokeService.success('Exito', 'El evento a sido eliminado');
					return of(true);
				}
				return of(false);
			})
		);
	}

	endEvent(idEvent: number): void {
		this._confirmBoxEvokeService
			.warning('Concierto', '¿Esta seguro de finalizar el Concierto?', 'Si', 'Cancelar')
			.pipe(
				concatMap((responseQuestion) =>
					responseQuestion.success ? this._eventApiService.finalizeProducto(idEvent) : EMPTY
				)
			)
			.subscribe(() => {
				this._toastEvokeService.success('Exito', 'El concierto a sido finalizado');
			});
	}

	updateForm(idEvent: number): Observable<IResponse<IResponseConcert>> {
		return this._eventApiService.getProducto(idEvent).pipe(
			map((response) => {
				const eventResponse = response.data;
				this.idField.setValue(eventResponse.id);
				this.titleField.setValue(eventResponse.nombre);
				this.descriptionField.setValue(eventResponse.description);
				this.dateField.setValue(new Date(eventResponse.dateEvent));
				//this.hourField.setValue(this._datePipe.transform(eventResponse.dateEvent, 'HH:mm')!);
				this.ticketsQuantityField.setValue(eventResponse.cantidad);
				this.priceField.setValue(eventResponse.unitPrice),
					this.genreField.setValue(eventResponse.tipoId),
					this.statusField.setValue(eventResponse.status ? STATUS_CRUD.ACTIVO : STATUS_CRUD.INACTIVO);
				this.imageField.setValue(eventResponse.imageUrl);

				this.crudMethod = CRUD_METHOD.UPDATE;
				return response;
			})
		);
	}

	saveEvent(): void {
		console.log("	QUIERE GRABAR");
		if (this.eventsFormGroup.valid) {
			console.log("VALIDO TODO");
			this._confirmBoxEvokeService
				.warning('Producto', '¿Esta seguro de guardar la información?', 'Si', 'Cancelar')
				.pipe(concatMap((responseQuestion) => (responseQuestion.success ? this._getMethod() : EMPTY)))
				.subscribe(() => {
					this._toastEvokeService.success('Exito', 'La información ha sido guardada.');
					this.eventsFormGroup.reset();
					this.crudSource.next(true);
				});
		}
	}

	private _getMethod(): Observable<IResponse<number>> {
		const idEvent = this.idField.value as number;
		const request = this._getRequest();
		return this.crudMethod === CRUD_METHOD.SAVE
			? this._eventApiService.createProducto(request)
			: this._eventApiService.updateProducto(idEvent, request);
	}

	/**
	 * En esta función vamos a retornar el evento que deseamos guardar o modificar; en el caso de las imagenes puede que al momento de seleccionar el evento para poder modificarlo solo modifiquen atributos de texto o número por lo tanto el valor de la imagen es solo una URL asi que no se debería de enviar, recuerden que el API necesita un base64 para crear una imagen.
	 * @param method
	 * @returns
	 */
	private _getRequest(): IRequestCreateUpdateConcert {
		const request: IRequestCreateUpdateConcert = <IRequestCreateUpdateConcert>{
			nombre: this.titleField.value,
			description: this.descriptionField.value,
			dateEvent: this._datePipe.transform(this.dateField.value, 'yyyy-MM-dd'),
			timeEvent: '12:00:00',
			cantidad: this.ticketsQuantityField.value,
			unitPrice: this.priceField.value,
			idTipo: this.genreField.value
		};

		const existHttpMitocode = this.imageField.value.search('https://mitocode.blob.core.windows.net');

		if (this.crudMethod === CRUD_METHOD.SAVE || (this.crudMethod == CRUD_METHOD.UPDATE && existHttpMitocode === -1)) {
			const base64 = this.imageField.value.split(',')[1];
			request.base64Image = base64;
			request.fileName = this.fileNameField.value!;
		}

		return request;
	}

	//#region
	get idField(): FormControl<number | null> {
		return this.eventsFormGroup.controls.id;
	}

	get titleField(): FormControl<string> {
		return this.eventsFormGroup.controls.nombre;
	}

	get descriptionField(): FormControl<string> {
		return this.eventsFormGroup.controls.description;
	}

	get dateField(): FormControl<Date> {
		return this.eventsFormGroup.controls.date;
	}



	get ticketsQuantityField(): FormControl<number> {
		return this.eventsFormGroup.controls.ticketsQuantity;
	}

	get priceField(): FormControl<number> {
		return this.eventsFormGroup.controls.price;
	}



	get genreField(): FormControl<number | null> {
		return this.eventsFormGroup.controls.genre;
	}

	get statusField(): FormControl<number> {
		return this.eventsFormGroup.controls.status;
	}

	get imageField(): FormControl<string> {
		return this.eventsFormGroup.controls.image;
	}

	get fileNameField(): FormControl<string | null> {
		return this.eventsFormGroup.controls.fileName;
	}
	//#endregion
}
