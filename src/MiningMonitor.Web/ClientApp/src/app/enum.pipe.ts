import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enum',
})
export class EnumPipe implements PipeTransform {
  public transform(enumType: { [key: string]: string | number }) {
    return Object.keys(enumType)
      .filter((key) => isNaN(Number(key)))
      .map((key) => ({ key, value: enumType[key] }));
  }
}
