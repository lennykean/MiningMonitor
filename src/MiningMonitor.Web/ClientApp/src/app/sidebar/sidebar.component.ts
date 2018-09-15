import { Component, OnInit, Input } from '@angular/core';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner.service';

@Component({
    selector: 'miningmonitor-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
    @Input()
    public width: string;
    public miners: Miner[] = [];

    constructor(
        private minerService: MinerService) {
    }

    public async ngOnInit() {
        this.miners = await this.minerService.GetAll();
    }
}
