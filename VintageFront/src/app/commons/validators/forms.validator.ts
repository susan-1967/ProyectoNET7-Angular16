import { AbstractControl, ValidationErrors } from '@angular/forms';

//Esta expresión regular validará si el texto contiene al menos una minuscula, mayuscula, número, symbolo y que la longitud sea como minimo igual a 8
const patternPassword = new RegExp('(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8}');

export const customPasswordValidator = (control: AbstractControl): ValidationErrors | null => {
	const value = control.value as string;

	if (!patternPassword.test(value)) {
		return { customPasswordValidator: true };
	}
	return null;
};

export const mayorEdad = (control: AbstractControl): ValidationErrors | null => {
	const value = control.value as number;

	if (value < 18) {
		return { errorEdad: true };
	}
	return null;
};
