import { PassportService } from 'src/app/services/passport.service';
import { AsyncValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';

export function UniqueUserUsernameValidator(PassportService:PassportService): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return PassportService.userNameExists(control.value);
    };
} 