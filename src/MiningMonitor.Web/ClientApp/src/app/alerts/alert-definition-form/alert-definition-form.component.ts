import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';

import { MinerService } from '../../miner/miner.service';
import { AlertDefinition } from '../../models/AlertDefinition';
import { Miner } from '../../models/Miner';

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
        enabled: true,
        parameters: {
            alertType: null
        }
    };
    @Input()
    public validationErrors: { [key: string]: string[] } = {};

    @Output()
    public save = new EventEmitter<AlertDefinition>();

    public miners: Observable<Miner[]>;

    constructor(
        private minerService: MinerService) {
    }

    public ngOnInit() {
        this.miners = this.minerService.miners;
    }

    public Submit() {
        this.save.emit(this.alertDefinition);
    }
}
