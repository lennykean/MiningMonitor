import { Component, OnInit } from '@angular/core';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner.service';

@Component({
    selector: 'miningmonitor-miners',
    templateUrl: './miners.component.html',
    styleUrls: ['./miners.component.scss']
})
export class MinersComponent implements OnInit {
    public miners: Miner[] = [];

    constructor(
        private minerService: MinerService) {
    }

    public async ngOnInit() {
        this.miners = await this.minerService.GetAll();
    }
}
