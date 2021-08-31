<script>
    import {createEventDispatcher} from 'svelte';
    const dispatch = createEventDispatcher();
    export let show=false;
    $:modalClass = show ? 'scale-100' : 'scale-0';
    
</script>
<div id="modal"
               class="fixed top-0 left-0 w-screen h-screen flex items-center justify-center bg-gray-500 bg-opacity-50 transform {modalClass} transition-transform duration-300">
    <div role="alert">
        <div class="bg-red-500 text-white font-bold rounded-t px-4 py-2">
            Danger
        </div>
        <div class="border border-t-0 border-red-400 rounded-b bg-red-100 px-4 py-3 text-red-700">
            <p>Are you sure you want to delete this bookshelf</p>
            <div class="flex flex-row justify-between">
                <button class="bg-red-500 hover:bg-red-700 text-white py-1 px-2 m-2 rounded-full" on:click={() => dispatch('continue')}>Okay</button>
                <button class="bg-green-500 hover:bg-green-700 text-white py-1 px-2 m-2 rounded-full" on:click={() => dispatch('cancel')}>Cancel</button>
            </div>
        </div>
    </div>
</div>
