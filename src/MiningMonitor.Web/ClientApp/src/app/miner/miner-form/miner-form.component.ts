import { Component, Input, Output, EventEmitter } from '@angular/core';

import { Miner } from '../../../models/Miner';

@Component({
    selector: 'mm-miner-form',
    templateUrl: './miner-form.component.html',
    styleUrls: ['./miner-form.component.scss']
})
export class MinerFormComponent {
    @Input()
    public miner: Miner = {
        displayName: null,
        address: null,
        port: null,
        collectData: null
    };
    @Input()
    public validationErrors: { [key: string]: string[] } = {};

    @Output()
    public save = new EventEmitter<Miner>();

    public Submit() {
        this.save.emit(this.miner);
    }
}
