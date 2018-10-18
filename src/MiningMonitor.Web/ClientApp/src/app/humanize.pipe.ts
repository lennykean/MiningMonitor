import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'humanize'
})
export class HumanizePipe implements PipeTransform {
    transform(value: string) {
        if (!value) {
            return value;
        }
        return value.replace(/([A-Z][a-z])/g, ' $1');
    }
}
