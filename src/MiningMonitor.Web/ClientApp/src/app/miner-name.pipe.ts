import { Pipe, PipeTransform } from '@angular/core';

import { MinerService } from './miner/miner.service';
import { map } from 'rxjs/operators';

@Pipe({
    name: 'minerName'
})
export class MinerNamePipe implements PipeTransform {
    constructor(
        private minerService: MinerService) {
    }

    public transform(minerId: string) {
        return this.minerService.miners
            .pipe(map(miners => {
                const miner = miners.find(m => m.id === minerId);
                return miner ? miner.name : minerId;
            }));
    }
}
