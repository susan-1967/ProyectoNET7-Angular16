import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastEvokeService } from '@costlydeveloper/ngx-awesome-popup';
import { DataItem, NgxChartsModule } from '@swimlane/ngx-charts';
import { PdfMakeWrapper, Table } from 'pdfmake-wrapper';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
import { SharedFormCompleteModule } from '../../../commons/shared/shared-form-complete.module';
import { IResponseGenre } from '../services/tipo/tipo-api-model.interface';
import { ReportsApiService } from '../services/service-index';

PdfMakeWrapper.setFonts(pdfFonts);
const pdf = new PdfMakeWrapper();

@Component({
	standalone: true,
	selector: 'app-maintenance-reports',
	templateUrl: './maintenance-reports.component.html',
	styleUrls: ['./maintenance-reports.component.scss'],
	imports: [SharedFormCompleteModule, NgxChartsModule],
	providers: [DatePipe, CurrencyPipe]
})
export default class MaintenanceReportsComponent {
	private _reportsApiService = inject(ReportsApiService);
	private _formBuilder = inject(FormBuilder);
	private _datePipe = inject(DatePipe);
	private _toastEvokeService = inject(ToastEvokeService);
	private _currencyPipe = inject(CurrencyPipe);

	listGenres: IResponseGenre[] = [];
	dataPie: DataItem[] = [];
	showReport = false;

	formGroup = this._formBuilder.nonNullable.group({
		dateInit: [new Date(), Validators.required],
		dateEnd: [new Date(), Validators.required]
	});

	clickQuery(): void {
		if (this.formGroup.invalid) {
			this._toastEvokeService.info(
				'Validaciones',
				'Asegurese de seleccionar el genero, la fecha de inicio y la fecha de fin'
			);
			return;
		}

		this._loadSale();
	}

	generatePdf(): void {
		const data = this.dataPie!.map((item) => {
			const mount = this._currencyPipe.transform(item.value, '2.2');
			return [item.name, mount];
		});
		const table = new Table(data).widths(['*', 100]).end;
		pdf.info({ title: 'Reporte MitoCode' });
		pdf.add('Reportes de Ventas');
		pdf.add(table);
		pdf.create().download('Reporte de Ventas');
	}

	private _loadSale() {
		const dateInit = this._datePipe.transform(this.formGroup.controls.dateInit.value, 'yyyy-MM-dd')!;
		const dateEnd = this._datePipe.transform(this.formGroup.controls.dateEnd.value, 'yyyy-MM-dd')!;

		this._reportsApiService.getDataSale(dateInit, dateEnd).subscribe((response) => {
			if (response && response.success && response.data.length > 0) {
				this.dataPie = response.data.map((item) => ({ name: item.productoName, value: item.total }));
				this.showReport = true;
			}
		});
	}

	clickClear(): void {
		this.formGroup.reset();
		this.showReport = false;
	}
}
