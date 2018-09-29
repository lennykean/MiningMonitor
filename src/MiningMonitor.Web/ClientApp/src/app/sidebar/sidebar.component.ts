import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner/miner.service';

@Component({
    selector: 'mm-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
    public miners: Observable<Miner[]>;

    constructor(
        private minerService: MinerService) {
    }

    public ngOnInit() {
        this.miners = this.minerService.miners;
    }
}
