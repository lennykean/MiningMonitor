import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AlertDefinition } from '../../../models/AlertDefinition';
import { Miner } from '../../../models/Miner';
import { MinerService } from '../../miner/miner.service';

@Component({
    selector: 'mm-alert-definition-form',
    templateUrl: './alert-definition-form.component.html',
    styleUrls: ['./alert-definition-form.component.scss']
})
export class AlertDefinitionFormComponent implements OnInit {
    @Input()
    public alertDefinition: AlertDefinition = {
        displayName: null,
        minerId: null,
        enabled: true
    };
    public miners: Observable<Miner[]>;

    constructor(
        private minerService: MinerService) {
    }

    public ngOnInit() {
        this.miners = this.minerService.miners;
    }
}
