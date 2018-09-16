import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner.service';

@Component({
    selector: 'miningmonitor-miners',
    templateUrl: './miners.component.html',
    styleUrls: ['./miners.component.scss']
})
export class MinersComponent implements OnInit {
    public miners: Observable<Miner[]>;

    constructor(
        private minerService: MinerService) {
    }

    public ngOnInit() {
        this.miners = this.minerService.miners;
    }
}
